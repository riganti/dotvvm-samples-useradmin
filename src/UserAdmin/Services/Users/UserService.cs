using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.EfCore.Services;
using DotVVM.Framework.Controls;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UserAdmin.Services.Users;

//public class UserService : EfCoreListPageService<IdentityUser, UserListModel, UserFilterModel>
//{
//    public UserService(DbContext dbContext) : base(dbContext)
//    {
//    }

//    protected override async Task<IQueryable<UserListModel>> ProjectAsync(IQueryable<IdentityUser> queryable)
//    {
//        return queryable
//            .Select(s => new UserListModel()
//            {
//                Id = s.Id,
//                UserName = s.UserName,
//                Email = s.Email,
//                PhoneNumber = s.PhoneNumber
//            });
//    }

//    protected override async Task PostProcessResults(IGridViewDataSet<UserListModel> items)
//    {
//        var userIds = items.Items.Select(i => i.Id).ToArray();

//        var userRoles = await dbContext.Set<IdentityUserRole<string>>()
//            .Join(dbContext.Set<IdentityRole>(), ur => ur.RoleId, r => r.Id,
//                (ur, r) => new { UserId = ur.UserId, RoleId = r.Id, RoleName = r.Name })
//            .Where(ur => userIds.Contains(ur.UserId))
//            .ToListAsync();

//        foreach (var item in items.Items)
//        {
//            item.Roles = string.Join(", ", userRoles.Where(ur => ur.UserId == item.Id).Select(ur => ur.RoleName));
//        }

//        await base.PostProcessResults(items);
//    }

//}