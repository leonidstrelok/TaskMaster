using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.DAL.Context;
using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.DAL.Seeds;

public class DataSeed
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<Client> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataSeed(AppDbContext dbContext, UserManager<Client> userManager, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Record()
    {
        await _dbContext.Database.MigrateAsync();
        
        var client = new Client()
        {
            UserName = "Leonid"
        };
        
        if (await _userManager.FindByNameAsync(client.UserName) == default)
        {
            await _userManager.CreateAsync(client, "P@ssw0rd1!");
        }
        
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleType.Admin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(RoleType.Director.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(RoleType.Customer.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(RoleType.Tester.ToString()));
        
            await _userManager.AddToRoleAsync(client, RoleType.Admin.ToString());
        }
    }
}