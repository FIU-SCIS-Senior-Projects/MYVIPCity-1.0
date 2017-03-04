﻿using System;
using System.Data.Entity.Migrations.Model;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using Ninject;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/Business")]
	public class BusinessController: ApiController {

		[Inject]
		public IKernel Kernel
		{
			get; set;
		}

		[Inject]
		public IBusinessManager BusinessManager
		{
			get;
			set;
		}

		[HttpGet]
		[Route("")]
		public async Task<IHttpActionResult> Index() {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dtos = BusinessManager.GetAllBusiness();
				return Ok(dtos);
			});
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IHttpActionResult> GetBusiness(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dto = BusinessManager.LoadById(id);
				return Ok(dto);
			});
		}

		[HttpGet]
		[Route("ByFriendlyId/{friendlyId}")]
		public async Task<IHttpActionResult> GetByFriendlyIdBusiness(string friendlyId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dto = BusinessManager.LoadByFriendlyId(friendlyId);
				return Ok(dto);
			});
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("")]
		public async Task<IHttpActionResult> AddBusiness(BusinessDto businessDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var savedBusinessDto = BusinessManager.Create(businessDto);
				return Ok(savedBusinessDto);
			});
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		[Route("")]
		public async Task<IHttpActionResult> UpdateBusiness(BusinessDto businessDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var updatedBusinessDto = BusinessManager.Update(businessDto);
				return Ok(updatedBusinessDto);
			});
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("SendPromoterInvitation")]
		public async Task<IHttpActionResult> SendPromoterInvitation(PromoterInvitationDto[] promoterInvitations) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var baseUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot);
				BusinessManager.SendPromoterInvitations(promoterInvitations, baseUrl.ToString());
				return Ok();
			});
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("GetPendingPromoterInvitations/{friendlyId}")]
		public async Task<IHttpActionResult> GetPendingPromoterInvitations(string friendlyId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var pendingInvitations = BusinessManager.GetPendingPromoterInvitations(friendlyId);
				return Ok(pendingInvitations);
			});
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("DeletePromoterInvitation/{id:int}")]
		public async Task<IHttpActionResult> DeletePromoterInvitation(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				BusinessManager.DeletePromoterInvitation(id);
				return Ok();
			});
		}
	}
}