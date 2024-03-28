using Microsoft.AspNetCore.Mvc;
using BookingSystem.Dto;
using BookingSystem.Extensions;
using System.Text;
using System.Text.Json;

namespace BookingSystem.Controllers
{
    public class ClientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _client => _httpClientFactory.CreateClient();

        public ClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Rewrite()
        {
            var msg = new StringBuilder();

            var requestDto = new
            {
                storeId = "S2212290010",
                mealPeriod = "dinner",
                peopleCount = 6
            };
            var jsonContent = JsonSerializer.Serialize(requestDto);
            var url = "https://www.feastogether.com.tw/api/booking/getStoreBookingSituation";


            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // 輸出發送的 request 內容到控制台
            msg.AppendLine("Request:" + jsonContent);

            // 發送 request 並等待回應
            var response = await _client.SendAsync(requestMessage);

            // 讀取回應內容
            var responseContent = await response.Content.ReadAsStringAsync();

            // 輸出回應內容到控制台
            msg.AppendLine("Response:" + responseContent);


            // 解析回應的 JSON
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(responseContent);

            var isCanBooking = false;
            // 檢查每個情況是否符合條件
            foreach (var situation in responseDto.result.situation)
            {
                if (!situation.isFull && int.TryParse(situation.surplusText, out int surplus) && surplus >= 6)
                {
                    msg.AppendLine($"你指定的時段【{requestDto.mealPeriod}】及人數【{requestDto.peopleCount}】人，在日期【{situation.date}】可以訂位!!");
                    isCanBooking = true;
                }
            }

            if (!isCanBooking)
                msg.AppendLine("沒有位可以訂!!");

            ViewBag.Message = msg.ToString().Replace(Environment.NewLine, "<br>");
            return View("Index");
        }

     
    


        public class RequestDto
        {
            public string storeId { get; set; }
            public string mealPeriod { get; set; }
            public int peopleCount { get; set; }
        }

    }
}
