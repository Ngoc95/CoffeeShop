using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class RequestService
    {
        public RequestService() { }
        private static RequestService _ins;

        public static RequestService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new RequestService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<(bool IsAdded, string Message)> AddRequest(RequestDTO requestDto)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    // Chuyển DTO sang entity
                    var newRequest = new REQUEST
                    {
                        EMP_ID = requestDto.EMP_ID,
                        REQ_TYPE = requestDto.REQ_TYPE,
                        REQ_DATE = DateTime.Now,
                        REQ_STATUS = requestDto.REQ_STATUS ?? "Chờ duyệt",
                        EMP_COMMENT = requestDto.EMP_COMMENT,
                        APPROVER_COMMENT = null,
                        APPROVED_BY = null,
                        APPROVED_DATE = null,
                        IS_DELETED = false
                    };

                    context.REQUESTs.Add(newRequest);
                    await context.SaveChangesAsync();

                    return (true, "Đã gửi đơn thành công!");
                }
            }
            catch (Exception ex) 
            {
                return (false, "Xảy ra lỗi" + ex);
            }
        }
    }
}
