using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MISA.WebFresher08.QLTS.DL;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    public class UsersController : BasesController<User>
    {
        #region Field

        private IUserBL _userBL;
        private IConfiguration _config;

        #endregion

        #region Constructor
        public UsersController(IConfiguration config, IUserBL userBL) : base(userBL)
        {
            _config = config;
            _userBL = userBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy người dùng bằng tên đăng nhập và mật khẩu
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Người dùng hợp lệ</returns>
        [HttpGet("signin/{username}/{password}")]
        public IActionResult GetByUsernamePassword([FromRoute] string username, [FromRoute] string password)
        {
            try
            {
                User user = _userBL.GetByUsernamePassword(username, password);
                // Xử lý dữ liệu trả về
                if (user.user_id != Guid.Empty)
                {
                    user.Token = GenerateToken(user);
                    return StatusCode(StatusCodes.Status200OK, user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ErrorResult(
                        QltsErrorCode.LoginFailed,
                        Resource.DevMsg_LoginFailed,
                        Resource.UserMsg_LoginFailed,
                        username+"/"+ password,
                        HttpContext.TraceIdentifier));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    QltsErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UserMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
