using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;

namespace business_logic_layer
{
    public class LoginBLL
    {
        private readonly IDbContextFactory _dbContextFactory;

        public LoginBLL(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<LoginModel> Authenticate(string username, string password)
        {
            //string connectionStringName = GetConnectionStringNameByUsername(username);
            var loginDAL = new LoginDAL(_dbContextFactory, username);

            LoginEnitiyModel loginEntity = await loginDAL.GetUserByEmail(username);
            if (loginEntity != null && loginEntity.password == password)
            {
                return new LoginModel()
                {
                    username = username,
                    password = password
                };
            }

            return null;
        }
    }
}
