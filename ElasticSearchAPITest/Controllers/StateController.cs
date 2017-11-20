using ElasticSearchAPITest.Data.Interfaces;
using ElasticSearchAPITest.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Threading.Tasks;

namespace ElasticSearchAPITest.Controllers
{
    [Produces("application/json")]
    [Route("test")]
    public class StateController : Controller
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }



        [Route("states")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var states = await _stateService.GetStatesAsync();

                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node);
                var client = new ElasticClient(settings);

                client.DeleteIndex("testentry");

                // create the index if it doesn't exist
                if (!client.IndexExists("state").Exists)
                    client.CreateIndex("state");


              var indexResponse = await client.IndexManyAsync(states, index: "state", type: "test");

                return Json("ok");
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }
        }

        [Route("query")]
        public IActionResult TestIndex(string state_id)
        {
            try
            {
                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node)
                    .InferMappingFor<State>(m => m
                    .IndexName("state")
                    .TypeName("test"));


                var client = new ElasticClient(settings);

                var response = client.Search<State>(s => s
                .Index("state")
                .Query(q =>
                q.Terms(t =>
                t.Field(state => state.countryCode.Suffix("*"))
                )));

                var result = response.DebugInformation;

                return Json(response.Documents);
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }
        }
    }
}