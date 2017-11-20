using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearchAPITest.Data.Interfaces;
using ElasticSearchAPITest.Data.Models;
using MySql.Data.MySqlClient;
using System;

namespace ElasticSearchAPITest.Data.Repository
{
    public class GeoTableViewService : IGeoTableViewService
    {
        #region Member

        private readonly IDbContextFactory _dbContextFactory;

        #endregion

        public GeoTableViewService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }



        public async Task<ICollection<GeoTableView>> GetGeoTableViewAsync()
        {
            try
            {
                List<GeoTableView> list = new List<GeoTableView>();

                using (MySqlConnection con = _dbContextFactory.CreateConnection())
                {
                    await con.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand("select g.* from GeoTableView g", con))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                list.Add(new GeoTableView()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    WardId = Convert.ToInt32(reader["WardId"]),
                                    Ward = reader["Ward"].ToString(),
                                    DistrictId = Convert.ToInt32(reader["DistrictId"]),
                                    District = reader["District"].ToString(),
                                    CityId = Convert.ToInt32(reader["CityId"]),
                                    City = reader["City"].ToString()
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
