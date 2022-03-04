using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.EfCore;
using DotVVM.DynamicData.Helpers.Services;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAdmin.Database;

namespace UserAdmin.Services.Roles;

public class RoleService : IListPageService<RoleModel, RoleFilterModel>, ISelectorDataProvider<RoleSelectorItem>, IDetailPageService<RoleModel, string>
{
    private readonly RoleManager<AppRole> roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task LoadItems(IGridViewDataSet<RoleModel> items, RoleFilterModel? filter)
    {
        await items.LoadFromQueryableAsync(roleManager.Roles.Select(r => new RoleModel()
        {
            Id = r.Id,
            Name = r.Name
        }));
    }

    public async Task<List<RoleSelectorItem>> GetSelectorItems()
    {
        return await roleManager.Roles
            .Select(r => new RoleSelectorItem()
            {
                Value = r.Id,
                DisplayName = r.Name
            })
            .ToListAsync();
    }

    public async Task<RoleModel> LoadItem(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        return new RoleModel()
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public async Task<RoleModel> InitializeItem()
    {
        return new RoleModel();
    }

    public async Task SaveItem(RoleModel item, string id)
    {
        if (id == null)
        {
            await roleManager.CreateAsync(new AppRole() { Name = item.Name });
        }
        else
        {
            var role = await roleManager.FindByIdAsync(id);
            role.Name = item.Name;
            await roleManager.UpdateAsync(role);
        }
    }

    public async Task DeleteItem(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        await roleManager.DeleteAsync(role);
    }
}