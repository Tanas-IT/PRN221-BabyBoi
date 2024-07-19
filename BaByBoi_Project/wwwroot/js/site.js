"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();

connection.on("ReceiveUserCount", function (count) {
    $('#userCount').text(count);
});

connection.on("ReceiveTotalRevenue", function (count) {
    $('#totalRevenue').text(count);
});

connection.on("ReceiveAllOrderCount", function (count) {
    $('#allOrderCount').text(count);
});

connection.on("ReceiveNewOrderCount", function (count) {
    $('#newOrderCount').text(count);
});

connection.on("ReceiveOrderStatusUpdate", function (orderId, newStatus) {
    var statusColor = getOrderStatusColor(newStatus);
    var statusText = getOrderStatusText(newStatus);

    $("#orderStatus-" + orderId).css("background-color", statusColor).text(statusText);
});

connection.on("ReceiveNewOrder", function () {
    var currentPath = window.location.pathname;
    if (currentPath === "/AdminPage/NewOrder") {
        loadDataAndRender("/AdminPage/NewOrderRealTime", renderOrderTable);
    }
});

connection.on("UpdateProductInCustomer", function (productId) {
    var currentPath = window.location.pathname;
    if (currentPath === "/CustomerViewPage/CusViewProduct") {
        var url = "/Admin/UpdateProductRealTime/" + productId;
        loadDataAndRender(url, renderNewProductItem);
    }
});



connection.start().catch(function (err) {
    console.error(err.toString());
});

function loadDataAndRender(url, renderFunction) {
    $.ajax({
        url: url,
        method: 'GET',
        success: function (result) {
            renderFunction(result);
        },
        error: function (error) {
            console.error('Error refreshing data:', error);
        }
    });
}

function renderOrderTable(orders) {
    var tableBody = $("#add-row tbody");
    var tr = '';
    $.each(orders, function (index, order) {
        var statusColor = getOrderStatusColor(order.Status); // Lấy màu sắc dựa trên trạng thái
        var statusText = getOrderStatusText(order.Status); // Lấy văn bản trạng thái

        tr += `<tr class="table-row">
            <td>${order.OrderCode}</td>
            <td>${order.OrderDate}</td>
            <td>${order.Address ? `${order.Address}` : 'N/A'}</td>
            <td>
                <span style="display: block;
                     border: 1px solid;
                     padding: 5px;
                     border-radius: 8px;
                     background-color: ${statusColor};
                     font-weight: bold;
                     text-align: center;
                     color: white;">
                    ${statusText}
                </span>
            </td>
            <td>${order.PaymentName ? order.PaymentName : 'N/A'}</td>
            <td>${order.VoucherPercent ? `${order.VoucherPercentt}%` : 'N/A'}</td>
            <td>${order.TotalProduct}</td>
            <td>${order.TotalPrice ? order.TotalPrice : ''}</td>
            <td>
                <div class="form-button-action">
                    <button type="button" title="" class="btn btn-link btn-lg" data-original-title="View Order Detail">
                        <div>
                            <a href="/AdminPage/OrderDetail?orderCode=${order.OrderCode}" style="text-decoration: none">
                                <i class="fa fa-eye"></i>
                            </a>
                        </div>
                    </button>
                </div>
            </td>
            <td>
                ${order.Status === 1 ?
                `<div class="form-button-action">
                        <form method="post"
                              action="/AdminPage/NewOrder?handler=ConfirmOrder&orderId=${order.OrderId}&customerId=${order.UserId}&pageIndex=1"
                              class="d-inline">
                                <input type="hidden" name="__RequestVerificationToken" value="${$('input[name="__RequestVerificationToken"]').val()}">
                            <button type="submit" class="btn btn-success btn-action m-2">
                                <i class="fa fa-check"></i>
                            </button>
                        </form>

                        <form method="post"
                              action="/AdminPage/NewOrder?handler=CancelOrder&orderId=${order.OrderId}&customerId=${order.UserId}&pageIndex=1"
                              onsubmit="return confirm('Bạn có chắc chắn muốn hủy đơn hàng này không?');"
                              class="d-inline">
                                <input type="hidden" name="__RequestVerificationToken" value="${$('input[name="__RequestVerificationToken"]').val()}">
                            <button type="submit" class="btn btn-danger btn-action m-2">
                                <i class="fa fa-times"></i>
                            </button>
                        </form>
                    </div>` : ''}
            </td>
        </tr>`;
    });

    tableBody.html(tr);
}

function renderNewProductItem(product) {
    var productElement = document.querySelector(`[data-product-id='${product.ProductId}']`);

    var productItemHtml = `
            <div class="product-item bg-light">
                <div class="product-img position-relative overflow-hidden">
                    <img src="${product.ImageUrl}" class="img-fluid w-100 rounded-top" alt="${product.ProductName}">
                    <div class="product-action" style="display:flex; justify-content:center">
                        <a style="padding:20px; margin-right:30px;" class="btn btn-outline-dark btn-square" href="/CustomerViewPage/CusViewProduct?handler=AddToCart&productId=${product.ProductId}&sizeId=${product.SizeId}"><i class="fa fa-shopping-cart"></i></a>
                        <a style="padding:20px; margin-right:30px;" class="btn btn-outline-dark btn-square" href="/CustomerViewPage/CusViewProduct?handler=ViewDetail&productId=${product.ProductId}&categoryId=${product.CategoryId}"><i class="far fa-eye"></i></a>
                    </div>
                </div>
                <div class="text-center py-4">
                    <p class="h6 text-decoration-none text-truncate product-name">${product.ProductName}</p>
                    <div class="d-flex align-items-center justify-content-center mt-2" style="flex-direction:column;height:3.5vw;">
                        <div class="d-flex" style="column-gap:0.5vw; height:100%;">
                            ${product.Discount && product.Discount !== 0 ?
            `<h5 style="text-decoration:line-through" class="product-old-price">${product.Price}</h5>
                                 <h5 style="font-size:1vw;color:red" class="product-discount">-${product.Discount}%</h5>` :
            `<h5 class="product-price">${product.Price}</h5>`}
                        </div>
                        ${product.Discount && product.Discount !== 0 ?
            `<h5 style="color:red" class="product-price-discount">${product.DiscountedPrice}</h5>` : ''}
                    </div>
                    <div class="d-flex align-items-center justify-content-center mb-1">
                        <small class="fa fa-star text-primary mr-1"></small>
                        <small class="fa fa-star text-primary mr-1"></small>
                        <small class="fa fa-star text-primary mr-1"></small>
                        <small class="fa fa-star text-primary mr-1"></small>
                        <small class="fa fa-star text-primary mr-1"></small>
                        <small>(99)</small>
                    </div>
                </div>
            </div>`;

    productElement.innerHTML = productItemHtml;
}

function getOrderStatusText(status) {
    switch (status) {
        case 1:
            return "Chờ chấp nhận";
        case 2:
            return "Đã huỷ";
        case 3:
            return "Bị từ chối";
        case 4:
            return "Đã xác nhận";
        default:
            return "Không xác định";
    }
}

function getOrderStatusColor(status) {
    switch (status) {
        case 1:
            return "#ffc107"; // Yellow
        case 2:
            return "#dc3545"; // Red
        case 3:
            return "#6c757d"; // Gray
        case 4:
            return "#28a745"; // Green
        default:
            return "#007bff"; // Blue
    }
}