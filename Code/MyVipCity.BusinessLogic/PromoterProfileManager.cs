using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using Censored;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class PromoterProfileManager: AbstractEntityManager, IPromoterProfileManager {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		[Inject]
		public IEmailService EmailService
		{
			get;
			set;
		}

		[Inject]
		public IPostsEntityManager PostsEntityManager
		{
			get;
			set;
		}

		public PromoterProfileDto[] GetPromoterProfiles(string userId) {
			var promoterProfiles = DbContext.Set<PromoterProfile>().Where(p => p.UserId == userId).ToArray();
			var promoterProfileDtos = ToDto<PromoterProfileDto[], PromoterProfile[]>(promoterProfiles);
			return promoterProfileDtos;
		}

		public PromoterProfileDto GetProfileById(int id) {
			// find the profile with the given id
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(id);
			// check if the profile was not found
			if (promoterProfile == null) {
				Logger.Warn($"PromoterProfile with id={id} not found");
				return null;
			}
			var promoterProfileDto = ToDto<PromoterProfileDto, PromoterProfile>(promoterProfile);
			return promoterProfileDto;
		}

		public PromoterProfileDto Update(PromoterProfileDto promoterProfileDto) {
			if (promoterProfileDto.Id == 0) {
				Logger.Error("Promoter profile Id cannot be 0");
				throw new InvalidOperationException("Promoter profile Id cannot be 0");
			}
			// only the owner of the profile can edit it
			ValidateCurrentUserIsOwnerOfPromoterProfile(promoterProfileDto.Id);
			// find the profile
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileDto.Id);
			// convert from dto to model
			var promoterProfileToUpdate = ToModel(promoterProfileDto, promoterProfile);
			// persist changes
			DbContext.SaveChanges();
			// convert back to dto
			var result = ToDto<PromoterProfileDto, PromoterProfile>(promoterProfileToUpdate);

			return result;
		}

		public string GetPromoterEmail(int id) {
			var promoter = DbContext.Set<PromoterProfile>().Find(id);
			if (promoter == null) {
				Logger.Warn($"Promoter with id: {id} not found");
				return null;
			}

			var email = DbContext.Database.SqlQuery<string>($"select Email from AspNetUsers where Id='{promoter.UserId}'").ToArray();

			return email.Length == 0 ? null : email[0];
		}

		public void Delete(int id) {
			var promoters = DbContext.Set<PromoterProfile>();
			var promoter = promoters.Find(id);
			if (promoter != null)
				promoters.Remove(promoter);
			DbContext.SaveChanges();
		}

		public ResultDto<bool> AddReview(int id, ReviewDto review) {
			// make sure owner of profile is not adding a review to itself
			ValidateCurrentUserIsNotOwnerOfPromoterProfile(id);
			if (string.IsNullOrWhiteSpace(review.ReviewerEmail)) {
				Logger.Warn("Review requires a reviewer email");
				return new ResultDto<bool> {
					Result = false,
					Messages = new[] { "Review requires a reviewer email" }
				};
			}

			var profile = DbContext.Set<PromoterProfile>().Find(id);
			if (profile == null) {
				Logger.Warn("Promoter not found");
				return new ResultDto<bool> {
					Result = false,
					Messages = new[] { "Promoter not found" }
				};
			}

			// set the date
			review.CreatedOn = DateTimeOffset.UtcNow;

			if (profile.Reviews != null) {
				var daysBeforeNewReview = 3;
				var recentReview = profile.Reviews.AsQueryable().FirstOrDefault(r => r.ReviewerEmail == review.ReviewerEmail && (review.CreatedOn - r.CreatedOn).Days < daysBeforeNewReview);
				if (recentReview != null) {
					Logger.Warn($"Review added to same promoter less than {daysBeforeNewReview} days ago");
					return new ResultDto<bool> {
						Result = false,
						Messages = new[] { $"A review was added to the same promoter less than {daysBeforeNewReview} days ago" }
					};
				}
			}

			// this is a new review
			review.Id = 0;

			// censor text in case there are some swear words
			if (!string.IsNullOrWhiteSpace(review.Text)) {
				var censored = new Censor(Utils.Constants.SwearWords);
				review.Text = censored.CensorText(review.Text);
			}

			var reviewModel = Mapper.Map<Review>(review);
			profile.Reviews.Add(reviewModel);
			DbContext.SaveChanges();

			// notify the promoter by email about the review
			EmailService.SendPromoterReviewNotificationEmailAsync(new PromoterReviewNotificationEmailModel {
				To = profile.Email,
				Subject = "New user review for profile " + profile.Business.Name,
				From = "hello@myvipcity.com",
				BusinessName = profile.Business.Name,
				Rating = review.Rating.ToString("0.#"),
				Comment = review.Text ?? ""
			});

			return new ResultDto<bool>(true);
		}

		public ResultDto<bool> RemoveReview(int id) {
			// find the review
			var review = DbContext.Set<Review>().Find(id);
			// check if the review does not exist
			if (review == null)
				return new ResultDto<bool>(false) { Messages = new[] { "Review not found" } };
			// remove the review
			DbContext.Set<Review>().Remove(review);
			// save the changes
			DbContext.SaveChanges();
			// return
			return new ResultDto<bool>(true);
		}

		public ReviewDto[] GetReviews(int id, int top) {
			return GetReviews(id, top, null);
		}

		public ReviewDto[] GetReviews(int id, int top, int afterReviewId) {
			return GetReviews(id, top, r => r.Id < afterReviewId);
		}

		// TODO: Only self promoter can edit own posts
		public PostDto AddOrUpdatePost(int id, PostDto postDto) {
			// only the owner of the profile can add a post to the profile
			ValidateCurrentUserIsOwnerOfPromoterProfile(id);
			return PostsEntityManager.AddOrUpdatePost<PromoterProfile>(id, postDto);
		}

		public ResultDto<bool> DeletePost(int id, int postId) {
			// only the owner of the profile can delete a post from profile
			ValidateCurrentUserIsOwnerOfPromoterProfile(id);
			return PostsEntityManager.DeletePost<PromoterProfile>(id, postId);
		}

		public PostDto[] GetPosts(int id, int top) {
			return PostsEntityManager.GetPosts<PromoterProfile>(id, top);
		}

		public PostDto[] GetPosts(int id, int top, int afterPostId) {
			return PostsEntityManager.GetPosts<PromoterProfile>(id, top, afterPostId);
		}

		private ReviewDto[] GetReviews(int id, int top, Expression<Func<Review, bool>> whereExpression) {
			var profile = DbContext.Set<PromoterProfile>().Find(id);
			if (profile == null)
				return null;
			IQueryable<Review> reviewsQueryable = profile.Reviews.AsQueryable();
			if (whereExpression != null)
				reviewsQueryable = reviewsQueryable.Where(whereExpression);

			var reviews = reviewsQueryable.OrderByDescending(r => r.Id).Take(top).ToArray();
			var reviewsDto = Mapper.Map<ReviewDto[]>(reviews);
			return reviewsDto;
		}

		private void ValidateCurrentUserIsOwnerOfPromoterProfile(int promoterProfileId) {
			var userId = ValidateUserIsAuthenticatedAndPromoterProfileExists(promoterProfileId);
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileId);
			// only the user associated to the profile can edit it
			if (promoterProfile.UserId != userId) {
				var msg = $"User with id: {userId} tried to edit Promoter Profile with Id: {promoterProfileId} which is associated to user: {promoterProfile.UserId}";
				Logger.Error(msg);
				throw new InvalidOperationException();
			}
		}

		private void ValidateCurrentUserIsNotOwnerOfPromoterProfile(int promoterProfileId) {
			var userId = ValidateUserIsAuthenticatedAndPromoterProfileExists(promoterProfileId);
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileId);

			if (promoterProfile.UserId == userId) {
				var msg = $"User with id: {userId} tried to perform an unauthorized operation on self promoter profile with id {promoterProfileId}";
				Logger.Error(msg);
				throw new InvalidOperationException();
			}
		}

		private string ValidateUserIsAuthenticatedAndPromoterProfileExists(int promoterProfileId) {
			if (Thread.CurrentPrincipal == null) {
				Logger.Error("There is no current principal");
				throw new InvalidOperationException();
			}
			// get the id of the currently authenticated user
			var userIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
			var userId = userIdentity.GetUserId();
			// find the promoter profile with the given id
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileId);
			// check if it does not exist
			if (promoterProfile == null) {
				var msg = $"Promoter profile with Id: {promoterProfileId} not found";
				Logger.Error(msg);
				throw new ObjectNotFoundException(msg);
			}
			return userId;
		}
	}
}
