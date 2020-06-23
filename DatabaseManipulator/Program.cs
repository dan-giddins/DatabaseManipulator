using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RIS.Integrations.Database;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManipulator
{
	class Program
	{
		private static async Task<int> Main()
		{
			// get context
			var context = ConfigureServices().GetRequiredService<RisIntegrationDbContext>();
			var worktribePeople = await context.WorktribePerson.Include(x => x.Appointments)
				.ToListAsync();
			// INSERT person (and all appointments, 2)
			context.RemoveRange(worktribePeople.Single(x => x.Id == "001111").Appointments);
			context.Remove(worktribePeople.Single(x => x.Id == "001111"));
			// UPDATE person
			// Login
			worktribePeople.Single(x => x.Id == "001568").Login = "TEST";
			// Archive
			worktribePeople.Single(x => x.Id == "002808").Archive = "TEST";
			// AuthType
			worktribePeople.Single(x => x.Id == "004695").AuthType = "TEST";
			// Email
			worktribePeople.Single(x => x.Id == "004949").Email = "TEST";
			// FirstName
			worktribePeople.Single(x => x.Id == "005061").FirstName = "TEST";
			// Surname
			worktribePeople.Single(x => x.Id == "005072").Surname = "TEST";
			// StartDate
			worktribePeople.Single(x => x.Id == "005120").StartDate = "TEST";
			// OrgUnitExtId
			worktribePeople.Single(x => x.Id == "005164").OrgUnitExtId = "TEST";
			// LoBirthDategin
			worktribePeople.Single(x => x.Id == "010984").BirthDate = "TEST";
			// JobTitle
			worktribePeople.Single(x => x.Id == "012748").JobTitle = "TEST";
			// HESAStaffIdentifier
			worktribePeople.Single(x => x.Id == "014638").HESAStaffIdentifier = "TEST";
			// INSERT appointment (new appointment, 1)
			context.Remove(worktribePeople.Single(x => x.Id == "022286").Appointments
				.Single(x => x.PostId == "022286_30108_19770228"));
			// INSERT appointments (new appointments, 2)
			context.RemoveRange(worktribePeople.Single(x => x.Id == "023537").Appointments);
			// UPDATE appointment
			// JobTitle
			worktribePeople.Single(x => x.Id == "023700").Appointments.First().JobTitle = "TEST";
			// Grade
			worktribePeople.Single(x => x.Id == "024109").Appointments.First().Grade = "TEST";
			// Spine
			worktribePeople.Single(x => x.Id == "024453").Appointments.First().Spine = "TEST";
			// OrgUnitExtId
			worktribePeople.Single(x => x.Id == "025117").Appointments.First().OrgUnitExtId = "TEST";
			// StartDate
			worktribePeople.Single(x => x.Id == "025139").Appointments.First().StartDate = "TEST";
			// Archive
			worktribePeople.Single(x => x.Id == "025162").Appointments.First().Archive = "TEST";
			// EndDate
			worktribePeople.Single(x => x.Id == "025243").Appointments.First().EndDate = "TEST";
			// AcademicEmployment
			worktribePeople.Single(x => x.Id == "025450").Appointments.First().AcademicEmployment = "TEST";
			// FixedTermContract
			worktribePeople.Single(x => x.Id == "025597").Appointments.First().FixedTermContract = "TEST";
			// Fte
			worktribePeople.Single(x => x.Id == "025955").Appointments.First().Fte = 999;
			// IsPrimary
			worktribePeople.Single(x => x.Id == "026192").Appointments.First().IsPrimary = "TEST";
			return await context.SaveChangesAsync();
		}

		private static IServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();
			// build config
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
				.AddEnvironmentVariables()
				.Build();
			// make it available via DI
			services.AddSingleton(x => configuration);
			// db services and so on
			services.AddEntityFrameworkSqlServer();
			services.AddDbContext<RisIntegrationDbContext>(options =>
				options.UseSqlServer(new SqlConnectionStringBuilder(
				configuration.GetConnectionString("RIS_Integration"))
				{
					Password = configuration["RisIntegrationDbPassword"]
				}.ConnectionString));
			return services.BuildServiceProvider();
		}
	}
}
