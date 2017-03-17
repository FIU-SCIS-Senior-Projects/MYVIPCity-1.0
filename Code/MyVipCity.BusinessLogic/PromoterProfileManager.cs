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
using MyVipCity.Domain;
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

			if (Thread.CurrentPrincipal == null) {
				Logger.Error("There is no current principal");
				throw new InvalidOperationException();
			}

			var userIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
			var userId = userIdentity.GetUserId();
			// find the promoter profile with the given id
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileDto.Id);
			// check if it does not exist
			if (promoterProfile == null) {
				Logger.Error($"Promoter profile with Id: {promoterProfileDto.Id} not found");
				throw new ObjectNotFoundException($"Promoter profile with Id: {promoterProfileDto.Id} not found");
			}
			// only the user associated to the profile can edit it
			if (promoterProfile.UserId != userId) {
				Logger.Error($"User with id: {userId} tried to edit Promoter Profile with id: {promoterProfile.Id} which is associated to user: {promoterProfile.UserId}");
				throw new InvalidOperationException();
			}
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

			return new ResultDto<bool>(true);
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

		public ReviewDto[] GetReviews(int id, int top) {
			return GetReviews(id, top, null);
		}

		public ReviewDto[] GetReviews(int id, int top, int afterReviewId) {
			return GetReviews(id, top, r => r.Id < afterReviewId);
		}
	}
}
