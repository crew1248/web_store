var onHover = false, startSlide = false, holder = $('.slider-img-holder'), timer;
var isTablet = false;
var onInit = true;

var getImages = function () {
    $.getJSON('/Product/All_Sl_Images', {}, function (data) {
        var t = 0;
        for (var i = 0; i < data.pids.length; i++) {
            if (t < 4) $('.slider-img-holder').append('<span class="slide-illu sl-active" data-pid="' + data.pids[i].pid + '" data-title="' + data.pids[i].title + '" data-sl-counter="' + i + '"  />');
            else $('.slider-img-holder').append('<span class="slide-illu" data-pid="' + data.pids[i].pid + '" data-title="' + data.pids[i].title + '" data-sl-counter="' + i + '"  />');
            t++;
        }

    });
}

$(document).ready(function () {

     //getImages();
    
    if ($('#screenWidth').val() < 992) isTablet = true;
   

    $('.sl-arrs img:first-child').on('click', function () {
        animSlides(false,isTablet);
    });
    $('.sl-arrs img:last-child').on('click', function () {
        animSlides(true,isTablet);
    });

    $('.slider-wrap').hover(function () {
        onHover = true;

    }, function () { onHover = false; });

    function StartSliding() {
        timer = setInterval(function () { animSlides(true, isTablet,onInit); }, 4000);
    }
    /*$('.sl-arrs').hover(function () {

        clearInterval(timer);
    }, function () {
        setInterval(function () { animSlides(true, isTablet); }, 4000);
    });*/
    animSlides(true, isTablet, onInit);
   
    
    onInit = false;
   // StartSliding();
    
});


function animSlides(direction,isTablet) {
   
    var slidesVisible = isTablet ? 3 : 5;
    var wrapper = $('.slider-wrap');
   // $('.sl-slide').css({ opacity: 1, display: 'none' });
   
  

    var settings = {
        list: $('.sl-slide').length-1,
        current: $('.sl-active'),                
        elem:0        
    }
    
    if (!onHover) {
        //if (settings.onInit) {
        //    $('.sl-slide').slice(0, slidesVisible).addClass('sl-active');
        //    settings.current = $('.sl-active');
        //}
        ShowSlides(direction);

        

       
        
        function ShowSlides(direction) {
            if (direction) {
                settings.elem = settings.current.last().index();
                var difference = settings.list - settings.elem;
            } else {
                settings.elem = settings.current.first().index();
                var difference = settings.elem;
            }
            Activate(settings.elem,direction);
            

            
            function Activate(index,dir){
              
                
                $('.sl-slide').removeClass('sl-active');
                               
                if (dir) {
                    //wrapper.css({ left: settings.current.last().outerWidth()+"px" });
                    if (settings.elem == settings.list) {
                      
                        $('.sl-slide').slice(0, slidesVisible).addClass('sl-active');
                    }
                    else if (difference < slidesVisible) {
                       
                        $('.sl-slide').slice(-slidesVisible).addClass('sl-active');
                    }
                    else {
                        $('.sl-slide').slice(settings.elem + 1, settings.elem + 1 + slidesVisible).addClass('sl-active');
                    }                  

                } else {
                   // wrapper.css({ left: settings.current.last().outerWidth() + "px" });
                    if (settings.elem == 0) {
                        
                        $('.sl-slide').slice(-slidesVisible).addClass('sl-active');
                    }
                    else if (difference < slidesVisible) {
                        $('.sl-slide').slice(0, slidesVisible).addClass('sl-active');
                    }
                    else {
                        $('.sl-slide').slice(settings.elem - slidesVisible,settings.elem ).addClass('sl-active');
                    }                   
                }
                settings.current = $('.sl-active');
                var idx = settings.current.first().index();
                var outer = 240;
                $('.slider-counter').html(settings.current.last().index() + 1 + " из " + (settings.list+1));
                //onInit ? wrapper.css({ left: -outer * idx+ "px" }) : wrapper.css({ left: (-outer * idx+1) + "px" });
                wrapper.css({ left: (-outer * idx) + "px" });
               
                //$('.sl-circle[data-slide-index="' + settings.elem + '"]').addClass('sl-active');
                
                
                //settings.current = $('.sl-active');
                
                

            }
            //$('#slide_' + index).css({ background: "url(/ImageData/GetProdImage?pid=" + $(this).data('pid') + "&width=400&height=300) no-repeat 50%", opacity: "0.5" }).animate({ opacity: "1" }, 1000);
            //$('#slide_' + index).attr('title', $(this).data('title'));
            //$('#slide_' + index).closest('a').attr('href', '/Product/Details/' + $(this).data('pid'));
        }

    }
}

