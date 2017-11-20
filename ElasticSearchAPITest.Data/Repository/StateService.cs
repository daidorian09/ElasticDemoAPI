using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearchAPITest.Data.Interfaces;
using ElasticSearchAPITest.Data.Models;
using MySql.Data.MySqlClient;
using System;

namespace ElasticSearchAPITest.Data.Core
{
    public class StateService : IStateService
    {
        #region Member

        private readonly IDbContextFactory _dbContextFactory;

        #endregion



        public StateService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        


        async Task<ICollection<State>> IStateService.GetStatesAsync()
        {
            try
            {
                List<State> list = new List<State>();

                using (MySqlConnection con = _dbContextFactory.CreateConnection())
                {
                    await con.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand("select s.* from State s", con))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                list.Add(new State()
                                {
                                    state_id = Convert.ToInt32(reader["state_id"]),
                                    countryCode = reader["countryCode"].ToString(),
                                    name_id = Convert.ToInt32(reader["name_id"]),
                                    stateCode = reader["stateCode"].ToString(),
                                    latitude = Convert.ToDecimal(reader["latitude"]),
                                    longitude = Convert.ToDecimal(reader["longitude"]),
                                    polygon = reader["polygon"].ToString()
                                });
                            }
                        }
                    }
                   await con.CloseAsync();
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }            
        }
    }
}
