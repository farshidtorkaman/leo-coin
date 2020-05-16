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
                    console.log(id + ": " + title);
                    $("#CityId").append("<option value='" + id + "'>" + title + " </option>");
                })
            },
            error: function(){
                toastr.error('خطایی رخ داد !');
            }
        })
    }
})