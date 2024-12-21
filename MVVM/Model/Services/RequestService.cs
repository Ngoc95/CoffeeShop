using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public async Task<List<RequestDTO>> GetAllRequest()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var reqList = (from s in context.REQUESTs
                                  where s.IS_DELETED == false
                                  select new RequestDTO
                                  {
                                      REQ_ID = s.REQ_ID,
                                      REQ_DATE = s.REQ_DATE,
                                      REQ_STATUS = s.REQ_STATUS,
                                      REQ_TYPE = s.REQ_TYPE,
                                      APPROVED_BY = s.APPROVED_BY,
                                      APPROVED_DATE = s.APPROVED_DATE,
                                      APPROVER_COMMENT = s.APPROVER_COMMENT,
                                      EMP_COMMENT = s.EMP_COMMENT,
                                      EMP_ID = s.EMP_ID,
                                      EMPLOYEE = s.EMPLOYEE,
                                      IS_DELETED = s.IS_DELETED,
                                  }).ToListAsync();
                    return await reqList;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool IsAdded, string Message)> AddRequest(RequestDTO requestDto)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
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
            catch 
            {
                return (false, "Xảy ra lỗi");
            }
        }
        public async Task<(bool, string)> EditRequest(REQUEST newReq, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var req = await context.REQUESTs.Where(p => p.REQ_ID == ID).FirstOrDefaultAsync();
                    if (req == null) return (false, "Không tìm thấy mã đơn");
                    req.APPROVER_COMMENT = newReq.APPROVER_COMMENT;
                    req.APPROVED_BY = newReq.APPROVED_BY;
                    req.APPROVED_DATE = newReq.APPROVED_DATE;
                    req.REQ_STATUS = newReq.REQ_STATUS;
                    await context.SaveChangesAsync();
                    return (true, "Đã lưu phản hồi");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi");
            }
        }
        public async Task<(bool, string)> DeleteRequest(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var req = await context.REQUESTs.Where(p => p.REQ_ID == ID).FirstOrDefaultAsync();
                    if (req.IS_DELETED == true) return (false, "Đã xóa đơn xin này rồi");
                    req.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi");
            }

        }
    }
}
