using OtoServisSatis.Data;
using OtoServisSatis.Data.Concrete;
using OtoServisSatis.Entities;
using OtoServisSatis.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoServisSatis.Service.Concrete
{
    public class UserService : UserRepository ,IUserService
    {
        public UserService(DataBaseContext context) : base(context)
        {
        }
    }
}
