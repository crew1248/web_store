
var imagePreview = function (newConfig) {
    /* CONFIG */

   var  imageCloseOff = false;
    var config = {

        xOffset: 100,
        yOffset: 50,
        width: 600,
        height: 800,
        isLoader: false,
        preloadTime: 500
    }
    //if (newConfig != null) {
    //    $.extend(config, newConfig);
    //}
    /* END CONFIG */
    $("img.preview").hover(function (e) {

        var preloadImg = config.isLoader ? "display:none" : "";
        var lrm = "<img src='/Content/images/loader.gif' class='prew-loader' />";
        $("body").append("<p id='preview'></p>");
        config.isLoader ? $("#preview").append(lrm) : "";
        //this.t = this.title;
        //this.title = "";
        //var c = (this.t != "") ? this.t : "";
        $('#preview span').empty();
        var c = this.title;
        var src = this.src;
        console.log(src);
        $id = this.id;
        $this = this;

        if ($(this).hasClass('grid-img')) {
            alert('g');
            $("body").append("<p id='preview'><span style='display:block;'>" + c + "</span><img class='prew-img'  height='400' width='600' style='" + preloadImg + "' src='/Admin/ImageData/GetProdImage?id=" + this.id + "&width=600&height=400' alt='Image preview' /></p>");
        }
        else if ($('img.preview').hasClass('post-img')) { $("#preview").append("<span style='display:block;'>" + c + "</span><img style='" + preloadImg + "' src='/Admin/ImageData/GetPostImage?id=" + this.id + "&width=600&height=400' alt='Image preview' /></p>"); }
        else if ($('img.preview').hasClass('files-img')) { $("#preview").append("<span style='display:block;'>" + c + "</span><img style='" + preloadImg + "' src='/Admin/ImageData/GetImgAsFile?src=~" + this.title + "&width=600&height=400' alt='Image preview' /></p>"); }
        else if ($('img.preview').hasClass('slider-img')) { $("#preview").append("<span style='display:block;width:" + config.width + "px'>" + c + "</span><img style='" + preloadImg + "' src='/Admin/Slider/GetSliderImage?id=" + this.id + "&width=800&height=600' alt='Image preview' /></p>"); }
        else if ($('img.preview').hasClass('gal-img')) { $("#preview").append("<span class='prew-img' style='display:block;width:" + config.width + "px'>" + c + "</span><img style='" + preloadImg + "' src='/Admin/ImageData/GetPhotoGalleryImage?id=" + $id + "&width=800&height=600&isGallery=true' alt='Превью' />"); }
        else if ($('img.preview').hasClass('gal-img-item')) { $("#preview").append("<span class='prew-img' style='display:block;width:" + config.width + "px'>" + c + "</span><img style='" + preloadImg + "' src='/Admin/ImageData/GetPhotoGalleryImage?id=" + this.id + "&width=600&height=600&isGallery=false' alt='Превью' />"); }
        else { $("body").append("<p id='preview' ><span style='display:block;width:" + config.width + "px'>" + c + "</span><img class='prew-img'  height='300' width='300' src='" + this.src + "' alt='Image preview' /></p>"); }

        //setTimeout(function () { $('.prew-loader').remove(); $('#preview img').fadeIn('slow'); }, config.preloadTime);

        //if ($('.prew-loader').length < 1) {
        //config.isLoader ? $("#preview").append(lrm) : "";
        // }
        $("#preview")
            .css("top", (e.pageY - config.yOffset) + "px")
            .css("left", (e.pageX + config.xOffset) + "px")
            .fadeIn("fast");


    }, function () {

        //this.title = this.t;
        if (!imageCloseOff)
            $('#preview').remove();
    });
    $("img.preview").mousemove(function (e) {
         
        var thisImg = $(this);

        setTimeout(function () { $('.prew-loader').remove(); $('#preview').css({ minWidth: thisImg.data('w') + "px", minHeight: thisImg.data('h') + "px" }); }, config.preloadTime);
        if ($(this).hasClass('gal-img-item')) config.xOffset = 350;
        
        if (e.pageX > 650) {
           
            $("#preview")
        .css("top", (e.pageY - config.xOffset) + "px")
        .css("left", (e.pageX - (config.yOffset + config.width)) + "px");

        }
        else if (e.pageY > 400) {
            $("#preview")
                .css("top", (e.pageY - 350) + "px")
                .css("left", (e.pageX + config.yOffset) + "px");
        }
        else {
            $("#preview")
         .css("top", (e.pageY - config.xOffset) + "px")
         .css("left", (e.pageX + config.yOffset) + "px");
        }
    });
};
//window.on('.cke_body', function () {
//    var h = $('#admincontent').innerHeight();
//    $('#categories').css({ 'height': h });
//});
