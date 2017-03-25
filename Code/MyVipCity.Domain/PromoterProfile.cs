using System;
using System.Collections.Generic;
using MyVipCity.Domain.Contracts;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain {

	public class PromoterProfile: IIdentifiable, IReviewable, IPostsEntity {

		private ICollection<Review> reviews;
		private ICollection<Post> posts;

		public PromoterProfile() {
			reviews = new List<Review>();
			posts = new List<Post>();
		}

		public int Id
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public virtual Business Business
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string NickName
		{
			get;
			set;
		}

		public string About
		{
			get;
			set;
		}

		public virtual ProfilePicture ProfilePicture
		{
			get;
			set;
		}

		public DateTimeOffset CreatedOn
		{
			get;
			set;
		}

		public virtual ICollection<Review> Reviews
		{
			get { return reviews; }
			protected set { reviews = value; }
		}

		/// <summary>
		/// List of posts for the profile.
		/// </summary>
		public virtual ICollection<Post> Posts
		{
			get { return posts; }
			protected set { posts = value; }
		}
	}
}
