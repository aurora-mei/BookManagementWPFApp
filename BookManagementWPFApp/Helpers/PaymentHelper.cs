using BookManagement.BusinessObjects;
using BookManagement.DataAccess.Repositories;
using BookManagementWPFApp.Constants;
using BookManagementWPFApp.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BookManagementWPFApp.Helpers;

public class PaymentHelper
{
    IConfiguration configuration;
    private string _clientId;
    private string _clientSecret;
    private string _authorizationURL;
    private string _baseUrl;
    private string createOrderURL;
    private string paypalOrderId = String.Empty;
    public PaymentHelper()
    {
        configuration = new ConfigurationBuilder()
            .AddJsonFile("Credentials.json", true, true)
            .Build();
        _clientId = configuration["PAYPAL:CLIENTID"];
        _clientSecret = configuration["PAYPAL:SECRET"];
        _baseUrl = configuration["PAYPAL:BASEURL"];
        _authorizationURL = $"{_baseUrl}/v1/oauth2/token";
        createOrderURL = $"{_baseUrl}/v2/checkout/orders";

    }

    public async Task<CreateOrderResponseDto> SendCreateOrderRequest(CreateOrderRequestDto createOrderRequestDto)
    {
        var httpClient = new HttpClient();
        var accessToken = await GetAuthorizationResponse();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken.access_token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var stringContent = new StringContent(JsonConvert.SerializeObject(createOrderRequestDto), Encoding.UTF8,
            "application/json");
        var responseMessage = await httpClient.PostAsync(createOrderURL, stringContent);

        var responseString = await responseMessage.Content.ReadAsStringAsync();
        // responseMessage.EnsureSuccessStatusCode();
        var responseJson = JsonConvert.DeserializeObject<CreateOrderResponseDto>(responseString);
        if (responseJson == null) throw new Exception("Response is null");
        // Set the id so that user can check for it status
        paypalOrderId = responseJson.id;
        return responseJson;
    }

    public CreateOrderRequestDto CreateOrderRequestDto(Order order)
    {
        var itemList = new List<CreateOrderRequestDto.Items>();
        if (order.OrderItems == null || order.OrderItems.Count() == 0)
        {
            throw new Exception("Order is empty");
        }

        foreach (var orderItem in order.OrderItems)
        {
            // Price of each book (No need to calculate the discount because I calculated before
            var discount = orderItem.Book.Discount;
            var singleDiscountEachBook = orderItem.Book.Price;
            if (discount != null)
            {
                // 5 quyen 20k => Giam 0.5 => moi quyen 10k
                singleDiscountEachBook = orderItem.Book.Price * discount.discountValue;
            }
            var usdPrice = singleDiscountEachBook / MyConstants.USD_DIFFERENCE;
            itemList.Add(new CreateOrderRequestDto.Items()
            {
                currency = MyConstants.CURRENCY,
                description =
                    $"A {(orderItem.Book.Category != null ? orderItem.Book.Category.CategoryName : "")} book by {orderItem.Book.Author.AuthorName ?? ""}",
                name = orderItem.Book.Title,
                quantity = orderItem.Quantity,
                unit_amount = new CreateOrderRequestDto.Unit_amount()
                {
                    value = usdPrice.ToString("F1"),
                    currency_code = MyConstants.CURRENCY,
                }
            });
        }
        // Convert VND to USD (No need to calculate for the discount here)
        var usdTotalPrice = order.TotalPrice / MyConstants.USD_DIFFERENCE;
        // Calculate the addresses of user first
        var addresses = new string[2];
        string homeAddr = "";
        string cityAddr = "";
        if (order.User.Address != null)
        {
            addresses = order.User.Address.Split(",");
            if (addresses.Length == 0)
            {
                throw new Exception("Address is empty");
            }
            else if (addresses.Length > 1)
            {
                homeAddr = addresses[0];
                cityAddr = addresses[1];
            }
        }
        else
        {
            homeAddr = "Dai hoc FPT";
            cityAddr = "Dai hoc FPT";
        }

        // Decide if user is using fast delivery or not
        bool isFastDelivery = order.ShippingMethod != null && !order.ShippingMethod.Equals("normal delivery", StringComparison.OrdinalIgnoreCase);
        var orderRequestDto = new CreateOrderRequestDto
        {
            intent = MyConstants.CAPTURE,
            purchase_units = new[]
            {
                // There will be only one purchase unit list
                new CreateOrderRequestDto.Purchase_units()
                {
                    reference_id = Guid.NewGuid().ToString(),
                    description = $"{order.User.Username} buy {order.OrderItems.Count()} books from book store",
                    shipping = new CreateOrderRequestDto.Shipping()
                    {
                        name = new CreateOrderRequestDto.Name()
                        {
                            full_name = order.User.Username
                        },
                        address = new CreateOrderRequestDto.Address
                        {
                            address_line_1 = !string.IsNullOrEmpty(homeAddr) ? homeAddr : order.User.Address,
                            admin_area_2 = !string.IsNullOrEmpty(cityAddr) ? cityAddr : order.User.Address,
                            country_code = MyConstants.COUNTRY_CODE // Change this later ?
                        }
                    },
                    // Amount total
                    amount = new CreateOrderRequestDto.Amount()
                    {
                        value = (isFastDelivery ? (usdTotalPrice + 0.5) : usdTotalPrice).ToString("F1"),
                        currency_code = MyConstants.CURRENCY,
                        breakdown = new CreateOrderRequestDto.Breakdown()
                        {
                            item_total = new CreateOrderRequestDto.Item_total()
                            {
                                value = usdTotalPrice.ToString("F1"),
                                currency_code = MyConstants.CURRENCY
                            },
                            shipping = new CreateOrderRequestDto.ShippingFee()
                            {
                                value = (isFastDelivery ? 0.5 : 0).ToString("F1", CultureInfo.InvariantCulture),
                                currency_code = MyConstants.CURRENCY
                            }
                        },
                    },
                    items = itemList.ToArray(),
                }
            }
        };
        return orderRequestDto;
    }

    public async Task<bool> ConfirmOrder(int orderId, IOrderRepository orderRepository)
    {
        if (string.IsNullOrEmpty(paypalOrderId)) throw new Exception("Please pay for the order first before confirming the order");
        // Send the request to paypal
        var accessToken = await GetAuthorizationResponse();
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        var confirmOrderURL = $"{_baseUrl}/v2/checkout/orders/{paypalOrderId}/capture";
        var response = await httpClient.PostAsync(confirmOrderURL, new StringContent("", Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            var order = await orderRepository.GetOrderAsync(o => o.OrderID == orderId);
            order.Status = MyConstants.STATUS_PAID_AND_CONFIRMED;
            orderRepository.UpdateOrder(order);
            return true;
        }
        if (!response.IsSuccessStatusCode)
        {
            var rspContent = await response.Content.ReadAsStringAsync();
            var captureResponse = JsonConvert.DeserializeObject<CaptureResponseFailDto>(rspContent);
            throw new Exception("The order is not paid, please try again");
        }
        return false; // Something went wrong
    }


    private async Task<AuthorizationResponseData> GetAuthorizationResponse()
    {
        var httpClient = new HttpClient();
        var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var keyValuePairs = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        };
        var response = await httpClient.PostAsync(_authorizationURL, new FormUrlEncodedContent(keyValuePairs));
        var responseAsString = await response.Content.ReadAsStringAsync();
        var authorizationResp = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);
        return authorizationResp;
    }
}