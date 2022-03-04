using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Services;
using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Services.Users;

//public class UserDetailService : IDetailPageService<UserDetailModel, string>
//{
//    private readonly UserManager<IdentityUser> userManager;

//    public UserDetailService(UserManager<IdentityUser> userManager)
//    {
//        this.userManager = userManager;
//    }

//    public async Task<UserDetailModel> LoadItem(string id)
//    {
//        var user = await userManager.FindByIdAsync(id);
//        return new UserDetailModel()
//        {
//            UserName = user.UserName,
//            Email = user.Email,
//            PhoneNumber = user.PhoneNumber,
//            Roles = (await userManager.GetRolesAsync(user)).ToList()
//        };
//    }

//    public async Task<UserDetailModel> InitializeItem()
//    {
//        return new UserDetailModel()
//        {
//            Roles = new List<string>()
//        };
//    }

//    public async Task SaveItem(UserDetailModel item, string id)
//    {
//        IdentityResult result;
//        IdentityUser user;
//        if (id == null)
//        {
//            user = new IdentityUser()
//            {
//                UserName = item.UserName,
//                Email = item.Email,
//                PhoneNumber = item.PhoneNumber
//            };
//            result = await userManager.CreateAsync(user);
//        }
//        else
//        {
//            user = await userManager.FindByIdAsync(id);
//            user.UserName = item.UserName;
//            user.Email = item.Email;
//            user.PhoneNumber = item.PhoneNumber;
//            result = await userManager.UpdateAsync(user);
//        }

//        var currentRoles = await userManager.GetRolesAsync(user);
//        await userManager.RemoveFromRolesAsync(user, currentRoles.Except(item.Roles));
//        await userManager.AddToRolesAsync(user, item.Roles.Except(currentRoles));

//        if (!result.Succeeded)
//        {
//            throw new Exception(string.Join(" ", result.Errors));
//        }
//    }

//    public async Task DeleteItem(string id)
//    {
//        var user = await userManager.FindByIdAsync(id);
//        await userManager.DeleteAsync(user);
//    }
//}