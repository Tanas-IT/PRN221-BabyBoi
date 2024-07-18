"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();

connection.on("ReceiveUserCount", function (count) {
    $('#userCount').text(count);
});

connection.on("ReceiveTotalRevenue", function (count) {
    $('#totalRevenue').text(count);
});

connection.on("ReceiveNewOrderCount", function (count) {
    $('.newOrderCount').text(count);
});

connection.start().catch(function (err) {
    console.error(err.toString());
});