using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;

namespace GAtreeWebStore
{
    public class ApplicationDBContext : DbContext

    {
        public DbSet<Data> GATreeData { get; set; }
        public DbSet<DataAttributes> GATreeDataAttributes { get; set; }
        public DbSet<TestData> GATreeTestsData { get; set; }
        public DbSet<TestDataRun> GATreeTestDataRun { get; set; }
        public DbSet<TestDataRunLog> GATreeTestDataRunLog { get; set; }
        public DbSet<ThinClient> GATreeThinClient { get; set; }
        public DbSet<Logging> GATreeLogging { get; set; }
        public DbSet<RecordTree> GATRecordOfTrees{ get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
    }
}
