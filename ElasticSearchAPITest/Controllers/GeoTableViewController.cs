using ElasticSearchAPITest.Data.Interfaces;
using ElasticSearchAPITest.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Threading.Tasks;

namespace ElasticSearchAPITest.Controllers
{
    [Produces("application/json")]
    [Route("geo")]
    public class GeoTableViewController : Controller
    {
        #region Member

        private readonly IGeoTableViewService _geoTableViewService;

        #endregion

        public GeoTableViewController(IGeoTableViewService geoTableViewService)
        {
            _geoTableViewService = geoTableViewService;
        }



        [Route("create")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var states = await _geoTableViewService.GetGeoTableViewAsync();

                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node);
                var client = new ElasticClient(settings);


                if (!client.IndexExists("geotable").Exists)
                    client.CreateIndex("geo");

                var indexResponse = await client.IndexManyAsync(states, index: "geotable", type: "geo");

                return Json(indexResponse.DebugInformation);
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }
        }


        [Route("query")]
        public async Task<IActionResult> SearchAsync(string ward)
        {
            try
            {
                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node)
                                                            .InferMappingFor<GeoTableView>(m => m
                                                            .IndexName("geotable")
                                                            .TypeName("geo"));

                var client = new ElasticClient(settings);

                var wardIds = ward.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //Multiple query string search

                var response = await client.SearchAsync<GeoTableView>(
                    geo => geo
                   .Query(q => q
                   .Bool(b => b
                   .Should(sh => sh
                   .Terms(t => t
                   .Field(f => f.WardId)
                   .Terms(wardIds)
                   )))));



                return Json(response.Documents);
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }

        }
    }
}