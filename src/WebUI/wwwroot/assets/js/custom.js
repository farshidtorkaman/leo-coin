'use strict';

$("#ProvinceId").change(function(){
    $("#CityId").empty().append("<option value='0'>انتخاب کنید</option>");
    let provinceId = this.value;
    if(provinceId !== 0) {
        $.ajax({
            url: '/panel/user/load_cities',
            data: { provinceId: provinceId },
            type: 'post',
            success: function(data){
                $.each(data, function(){
                    let id = "";
                    let title = "";
                    $.each(this, function(key, value) {
                        if(key === "id") {
                            id = value;
                        }
                        if(key === "title") {
                            title = value;
                        }
                    });
                    $("#CityId").append("<option value='" + id + "'>" + title + " </option>");
                })
            },
            error: function(){
                toastr.error('خطایی رخ داد !');
            }
        })
    }
});

$(".confirmPurchase").click(function(){
    $(".modal-content").empty();
    let fullname = $(this).data("fullname");
    let amount = $(this).data("amount");
    let purchaseId = $(this).data("purchaseid");
    $.get("/admin/confirm_purchase", { purchaseId: purchaseId, fullName: fullname, amount: amount}, function(data){
        $(".modal-content").append(data);
        $(".confirmRejectModal").modal('show');
    })
})

$(".rejectPurchase").click(function(){
    $(".modal-content").empty();
    let fullname = $(this).data("fullname");
    let amount = $(this).data("amount");
    let purchaseId = $(this).data("purchaseid");
    $.get("/admin/reject_purchase", { purchaseId: purchaseId, fullName: fullname, amount: amount}, function(data){
        $(".modal-content").append(data);
        $(".confirmRejectModal").modal('show');
    })
})

$(".confirmSell").click(function(){
    $(".modal-content").empty();
    let fullname = $(this).data("fullname");
    let amount = $(this).data("amount");
    let sellId = $(this).data("sellid");
    $.get("/admin/confirm_sell", { sellId: sellId, fullName: fullname, amount: amount}, function(data){
        $(".modal-content").append(data);
        $(".confirmRejectModal").modal('show');
    })
})

$(".rejectSell").click(function(){
    $(".modal-content").empty();
    let fullname = $(this).data("fullname");
    let amount = $(this).data("amount");
    let sellId = $(this).data("sellid");
    $.get("/admin/reject_sell", { sellId: sellId, fullName: fullname, amount: amount}, function(data){
        $(".modal-content").append(data);
        $(".confirmRejectModal").modal('show');
    })
});