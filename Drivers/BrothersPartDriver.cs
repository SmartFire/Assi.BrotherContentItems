using System.Linq;
using Assi.BrotherContentItems.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Feeds;
using System.Web.Routing;

namespace Assi.BrotherContentItems.Drivers {
    public class BrothersPartDriver : ContentPartDriver<BrothersPart> {
        private readonly IContentManager _contentManager;
		private readonly IFeedManager _feedManager;

        public BrothersPartDriver(IContentManager contentManager, IFeedManager feedManager) {
            _contentManager = contentManager;
			_feedManager = feedManager;
        }

        protected override DriverResult Display(BrothersPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Brothers", () => {
				var thisCommonPart = part.ContentItem.As<CommonPart>();
                var container = thisCommonPart.Container.ContentItem;
                var query = _contentManager
                .Query(VersionOptions.Published)
                .Join<CommonPartRecord>().Where(x => x.Container.Id == container.Id)
                .Join<ContainablePartRecord>().OrderByDescending(x => x.Position);

                var metadata = container.ContentManager.GetItemMetadata(container);
                if (metadata != null) {
                    _feedManager.Register(metadata.DisplayText, "rss", new RouteValueDictionary {{"containerid", container.Id}});
                }

                // Retrieving pager parameters.
                /*var queryString = _orchardServices.WorkContext.HttpContext.Request.QueryString;

                var page = 0;
                // Don't try to page if not necessary.
                if (part.Paginated && queryString["page"] != null) {
                    Int32.TryParse(queryString["page"], out page);
                }

                var pageSize = part.PageSize;
                // If the container is paginated and pageSize is provided in the query string then retrieve it.
                if (part.Paginated && queryString["pageSize"] != null) {
                    Int32.TryParse(queryString["pageSize"], out pageSize);
                }

                var pager = new Pager(_siteService.GetSiteSettings(), page, pageSize);

                var pagerShape = shapeHelper.Pager(pager).TotalItemCount(query.Count());
                var startIndex = part.Paginated ? pager.GetStartIndex() : 0;
                */
				var pageOfItems = query.List();
				
                var listShape = shapeHelper.List();
                listShape.AddRange(pageOfItems.Select(item => _contentManager.BuildDisplay(item, "Summary")));
                listShape.Classes.Add("content-items");
                listShape.Classes.Add("list-items");

                return shapeHelper.Parts_Brothers(
                    List: listShape,
                    Parent: container
                );
            });
        }
		
    }
}