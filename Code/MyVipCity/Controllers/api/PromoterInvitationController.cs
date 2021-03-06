﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/PromoterInvitation")]
	public class PromoterInvitationController: ApiController {

		public PromoterInvitationController(IPromoterInvitationManager promoterInvitationManager) {
			PromoterInvitationManager = promoterInvitationManager;
		}

		public IPromoterInvitationManager PromoterInvitationManager
		{
			get;
			set;
		}
		
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("SendInvitation")]
		public async Task<IHttpActionResult> SendPromoterInvitation(PromoterInvitationDto[] promoterInvitations) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				// var baseUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot);
				var baseUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AcceptPromoterInvitation?friendlyId={0}";
				var result = PromoterInvitationManager.SendPromoterInvitations(promoterInvitations, baseUrl);
				return Ok(result);
			});
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("GetPendingInvitations/{friendlyId}")]
		public async Task<IHttpActionResult> GetPendingPromoterInvitations(string friendlyId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var pendingInvitations = PromoterInvitationManager.GetPendingPromoterInvitations(friendlyId);
				return Ok(pendingInvitations);
			});
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("DeleteInvitation/{id:int}")]
		public async Task<IHttpActionResult> DeletePromoterInvitation(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				PromoterInvitationManager.DeletePromoterInvitation(id);
				return Ok();
			});
		}
	}
}