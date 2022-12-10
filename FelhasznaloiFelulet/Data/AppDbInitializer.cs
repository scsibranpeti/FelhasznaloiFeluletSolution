using FelhasznaloiFelulet.Models;

namespace FelhasznaloiFelulet.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();
                
                //Bank
                if (!context.Bank.Any())
                {
                    context.Bank.AddRange(new List<Bank>()
                    {
                        new Bank()
                        {
                            Swift= "MKKBHUHBXXX",
                            Name= "MKB Bank Nyrt.",
                            SeatAddress= "Budapest, Váci utca 38"
                        },
                        new Bank()
                        {
                            Swift= "OTPVHUHB",
                            Name= "OTP Bank Nyrt.",
                            SeatAddress= "1051 Budapest, Nádor utca 16."
                        },
                        new Bank()
                        {
                            Swift= "UBRTHUHB",
                            Name= "Raiffeisen Bank",
                            SeatAddress= "1133 Budapest, Váci út 116-118."
                        },
                        new Bank()
                        {
                            Swift= "CITIHUHX",
                            Name= "CitiBank Magyarország",
                            SeatAddress= "1133 Budapest, Váci út 80."
                        }
                    });
                    context.SaveChanges();
                    //Countries
                    if (!context.Countries.Any())
                    {
                        context.Countries.AddRange(new List<Countries>()
                        {
                            new Countries()
                            {
                                ID= "A",
                                Name="Ausztria",
                                isEU=true,
                                CountryTel=43
                            },
                            new Countries()
                            {
                                ID = "ET",
                                Name = "Egyiptom",
                                isEU =false,
                                CountryTel = 30
                            },
                            new Countries()
                            {
                                ID = "H",
                                Name = "Magyarország",
                                isEU =true,
                                CountryTel =36
                            }
                        });
                        context.SaveChanges();
                    }    

                }
            }
        }
    }
}
