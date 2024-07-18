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
            console.log(result)
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
    var productElement = document.querySelector(`[data-product-id='${product.productId}']`);

    if (productElement) {
        // Nếu sản phẩm đã tồn tại, cập nhật thông tin sản phẩm
        productElement.querySelector('.product-img img').src = product.imageUrl;
        productElement.querySelector('.product-name').innerText = product.productName;
        productElement.querySelector('.product-price').innerText = product.price;

        if (product.discount) {
            productElement.querySelector('.product-discount').innerText = `${product.discount}% off`;
        } else {
            productElement.querySelector('.product-discount').innerText = '';
        }
    }
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