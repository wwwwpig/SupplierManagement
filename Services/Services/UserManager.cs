using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFirst.Entities;
namespace WebFirst.Services
{
public class UserManager : Repository<User>
{
        /// <summary>
        /// 通过用户主键获取该用户的所有权限名称
        /// </summary>
        /// <param name="id">用户主键ID</param>
        /// <returns>该用户多个角色去重复后的所有权限</returns>
        public List<string> GetPermissionsByUserID(int id)
        {
            var permissionNames = Db.Queryable<UserRole>()
                .InnerJoin<Role>((ur, r) => ur.RoleID == r.Id)
                .InnerJoin<RolePermission>((ur, r, rp) => r.Id == rp.RoleID)
                .InnerJoin<Permission>((ur, r, rp, p) => rp.PermissionID == p.Id)
                .Where((ur, r, rp, p) => ur.UserID == id)
                .Select((ur, r, rp, p) => p.MenuName)
                .Distinct()
                .ToList();
            return permissionNames;
        }

        public List<User> GetUsersByQuery(string userID, string userName)
        {
            var query = base.AsQueryable();
            if (userID != "")
            {
                //query = query.Where(it => it.Id == id);
                query = query.Where(it => it.UserID.Contains(userID));
            }
            if (userName != "")
            {
                query = query.Where(it => it.UserName.Contains(userName));
            }
            return query.OrderBy(it => it.UserID).ToList();
        }
        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool HasUser(string userID)
        {
            return base.GetSingle(it => it.UserID == userID) != null;
        }

        public bool ResetPwd(List<User> users,string newPwd)
        {

            foreach (var user in users)
            {
                user.Pwd = newPwd;
            }
            return Db.Updateable<User>(users).UpdateColumns(it => new {it.Pwd}).ExecuteCommand()>0;
            

            
        }

        public bool UpdatePwd(User user, string newPwd)
        {
            user.Pwd = newPwd;
            return Db.Updateable(user).ExecuteCommand() > 0;
        }

        public bool DeleteUser(List<User> users)
        {
            if (users == null || users.Count == 0) return false;

            try
            {
                Db.Ado.UseTran(() =>
                {
                    // 先删除该角色的映射
                    Delete(users);
                    // 再删除该角色
                    var userIds = users.Select(u => u.Id).ToList();
                    if (userIds.Count > 0)
                    {
                        Db.Deleteable<UserRole>().Where(ur => userIds.Contains(ur.UserID ?? 0)).ExecuteCommand();
                    }
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        //当前类已经继承了 Repository 增、删、查、改的方法

        //这里面写的代码不会给覆盖,如果要重新生成请删除 UserManager.cs


        #region 教学方法
        /// <summary>
        /// 仓储方法满足不了复杂业务需求，业务代码请在这里面定义方法
        /// </summary>
        public void Study()
    {
	     
	   /*********查询*********/

        var data1 = base.GetById(1);//根据ID查询
        var data2 = base.GetList();//查询所有
        var data3 = base.GetList(it => 1 == 1);  //根据条件查询  
        //var data4 = base.GetSingle(it => 1 == 1);//根据条件查询一条,如果超过一条会报错

        var p = new PageModel() { PageIndex = 1, PageSize = 2 };// 分页查询
        var data5 = base.GetPageList(it => 1 == 1, p);
        Console.Write(p.TotalCount);//返回总数

        var data6 = base.GetPageList(it => 1 == 1, p, it => SqlFunc.GetRandom(), OrderByType.Asc);// 分页查询加排序
        Console.Write(p.TotalCount);//返回总数
     
        List<IConditionalModel> conModels = new List<IConditionalModel>(); //组装条件查询作为条件实现 分页查询加排序
        conModels.Add(new ConditionalModel() { FieldName= typeof(User).GetProperties()[0].Name, ConditionalType = ConditionalType.Equal, FieldValue = "1" });//id=1
        var data7 = base.GetPageList(conModels, p, it => SqlFunc.GetRandom(), OrderByType.Asc);

        base.AsQueryable().Where(x => 1 == 1).ToList();//支持了转换成queryable,我们可以用queryable实现复杂功能

 

        /*********插入*********/
        var insertData = new User() { };//测试参数
        var insertArray = new User[] { insertData };
        base.Insert(insertData);//插入
        base.InsertRange(insertArray);//批量插入
        var id = base.InsertReturnIdentity(insertData);//插入返回自增列
        base.AsInsertable(insertData).ExecuteCommand();//我们可以转成 Insertable实现复杂插入



		/*********更新*********/
	    var updateData = new User() {  };//测试参数
        var updateArray = new User[] { updateData };//测试参数
        base.Update(updateData);//根据实体更新
        base.UpdateRange(updateArray);//批量更新
        //base.Update(it => new User() { ClassName = "a", CreateTime = DateTime.Now }, it => it.id==1);// 只更新ClassName列和CreateTime列，其它列不更新，条件id=1
        base.AsUpdateable(updateData).ExecuteCommand();  //转成Updateable可以实现复杂的插入



		/*********删除*********/
	    var deldata = new User() {  };//测试参数
        base.Delete(deldata);//根据实体删除
        base.DeleteById(1);//根据主键删除
        base.DeleteById(new int[] { 1,2});//根据主键数组删除
        base.Delete(it=>1==2);//根据条件删除
        base.AsDeleteable().Where(it=>1==2).ExecuteCommand();//转成Deleteable实现复杂的操作
    } 
    #endregion

 
 }
}