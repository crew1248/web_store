/*=============================  Image Preview  ============================================== */





//var initPartialViews = function () {

//    $.get('/Config/GetPartials', function (html) {
//        $('body').prepend(html);
//    });
//}


//var expandPartialBlock = function () {

//    var fcont = $('.foreign-cont');
//    $('.forg-btn').hover( function (e) {
    
//        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
//    });
//    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {

//        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
//    });
//    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
//        fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
//    });
//    $(document).on('mouseout', 'body', function () {

//        if(fcont.css('marginTop')=='20px')fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
//    });
//}

var footNav = function () {
    $mainNav = $('.tmenu');
    $serviceNav = $('.bmenu');
    $footWrap = $('.f-menu');
    $mobnavWrap = $('.mob-nav-target');
    $cat = $('#catsList');


    $serviceNav.clone().appendTo($mobnavWrap);
    $mainNav.clone().appendTo($footWrap);
    $serviceNav.clone().appendTo($footWrap);
    $cat.clone().appendTo($footWrap);
    $('.f-menu ul').attr('class', 'foot-nav');
    $('.f-menu ul').removeAttr('id');
    $('.f-menu ul i').remove();
    $('.foot-nav li span').html(function (i, h) {
        return h.replace(/&nbsp;/g, '');
    });
    $('<li>Разделы</li>').insertBefore($('.f-menu ul:eq(0) li:eq(0)'));
    $('<li>Услуги</li>').insertBefore($('.f-menu ul:eq(1) li:eq(0)'));
    $('<li>Каталог</li>').insertBefore($('.f-menu ul:eq(2) li:eq(0)'));
}


var cartIsHidden = false;
var loader = $('<img class="submit-loader" src="/Content/ajax-loaders/horizont/89.GIF" >');
var loader1 = $('<img class="submit-loader" src="/Content/ajax-loaders/horizont/main-loader1.GIF" >');
var pcartsubmit = $('.loader-state');
var cartSummary = $('#cart__summary');
var cartIndex = $('.cart-cont').length;
var partialTimeout = null;

var cartCheckout = function (step,isAuth) {
    $.ajax({
        beforeSend:function(){addLoader();},
        url: '/Checkout/Processing',
        type: 'post',
        success: function (data) {
            if (data.type == 0){
                XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
                closeLoader();
            }
           else       
                location.replace('/Checkout/Index?step=2');
           
        }
    })
}
var initCartEvents = function () {
    var fcont = $('.foreign-cont');
    var hasItems = $('.forg-menu .item-row').length;
    $('.forg-btn').click(function (e) {
        if (!hasItems) return false;
            $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    //$('.forg-btn').click(function (e) { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); });

    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {
        if (!hasItems) return false;
        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
        if (!cartIsHidden) return false;
        fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
   // $(document).on('click', '*[data-event-type="cart__remove"]', removeFromCart);
}
var authAttach = function () {
  XN.BuildModal('#modal-7', '/Account/Mauth');
};
var addToCart = function () {
    
    $this = $(this);
    
    addLoader();
    XN.Inscreaser.Close();
    var id = $this.hasClass('xn-cart-option') ? $this.data('pid') : $this.closest('.xn-listview-item').data('id');
    
    $.post('/Cart/AddToCart', { prodId: id }, function (data) {
        updateSummary(data.count);
        ExpandPartialCart(id, data.title, data.price, data.count, data.total);
        initCartEvents();
        //updateCartEvent(id,true);
        setTimeout(function () { closeLoader(); }, '500');
        //partialOff();
    });
}
var toTheCart = function () {

    document.location.replace('/Checkout?step=1');
}
var removeFromCart = function () {

    addLoader();
    var pid = $(this).closest('.item-row').data('pid');
   
    var row = $(this).closest('.item-row');
    $.post("/Cart/RemoveFromCart", { prodId: pid }, function (data) {
        $('.forg-menu tr[data-pid="' + pid + '"]').remove();
        updateSummary(data.count);
        updateTotal(data.total, data.count);
        //updateCartEvent(pid, false);
        //$('.cart__view[data-pid="' + pid + '"]').addClass('cart__add').removeClass('cart__view').html('Купить');
        setTimeout(function () { closeLoader(data.total, data.count); row.remove(); }, '200');
    });
}
function partialOff() {
    clearTimeout(partialTimeout);
    partialTimeout = setTimeout(function () { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); }, '4500');
};
function partialOffNow() {
    $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
}
function updateSummary(val) {
    
    $('.top-cart-total').hide();
    $('.top-cart-total i').css('display', '');
    //var res = isPlus ? cartSummary.text() + val : cartSummary.text() - val;
    $('#cart__summary').html(val);

}
function updateTotal(total, quant) {
    if ($('.cart-cont').length) {

    }
    else if (quant > 0) {
        $('.partial-cart__summary').html(quant);
        $('.partial-cart__total').html(total);
    }
    else {
        //$('.top-cart-total i').hide();
        
        $('.partial-cart__total').html('');
        $('.partial-cart__summary').html('корзина пуста');
        partialOffNow();
    }
}

function ExpandPartialCart(id, title, p, c, t) {
    var subTitle = title.length > 25 ? title.substring(0, 25) + '...' : title;
    var src = $('#mpic_' + id).attr('src');
    var img = $('<img src="'+src+'" />');
    $('.forg-menu').append(
        '<tr class="item-row" data-pid="' + id + '">' +
        '<td><img class="forg-img" src="'+src+'" /></td>' +
        '<td><span>' + subTitle + '</span><span class="top-cart-remove" data-event-type="cart__remove">удалить</span></td>' +
        '<td>' + p + ' </td>' +
        '</tr>'
        );

    updateTotal(t, c);
    $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    //$(document).on('click', '.forg-menu tr[data-pid="'+id+'"] span[data-event-type="cartRemove"]', removeFromCart);
    //closeLoader();
}
function updateCartEvent(id, a) {

    if (a) {
        $('.prod_id-' + id).html('товар добавлен');
        $('.prod_id-' + id).removeClass('cart__add').addClass('cart__view');
    } else {
        $('.prod_id-' + id).html('Купить');
        $('.prod_id-' + id).removeClass('cart__view').addClass('cart__add');
    }
}
function summaryCartInit() {
    $.get('/Cart/CartSummary', function (data) {
        $('#cart__summary').html(data);
        //initCartEvents();
    });
}
function partialCartInit() {
    if (!$('#ch-content').length) {
        $.get('/Cart/CartPartial', function (data) {
            $('.pbasket').append(data);
            initCartEvents();
        });
    }
}
function addLoader() {
    
    if ($('.cart-cont').length) {
        $('.cart-sum-td').html(loader1);
    } else {
        //alert(submit.attr('class'));
        $('.top-cart-checkout').prepend(loader);
        //alert(pcartsubmit.text());
        $('#cart__partial .loader-state').text('');
    }
}
function PriceFormatted(price) {
    return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1 ");
}

function closeLoader(s, t) {
    
    if ($('.cart-cont').length) {
        //<td class="cart-sum-td" colspan="2">Общая цена: <span class="pbasket-total">@Model.Cart.TotalValue().ToString("0 000")</span> <i class="fa fa-rouble"></i></td>
        
        $('.submit-loader').remove();
        //$('.pbasket-total').html(data.s);
        // $('.item-total').html(data.t);
        
        $('.cart-sum-td').html('Общая цена: <span class="pbasket-total">' + PriceFormatted(s) + '</span> <i class="fa fa-rouble"></i>');
    }
    else {
        $('.top-cart-total').fadeIn();
        $('.submit-loader').remove();
        $('#cart__partial .loader-state').text('Оформить заказ');
    }
}
var cartItemDecrease = function () {
    $this = $(this);
    if ($(this).hasClass('item-disabled')) { return false; }
    $(this).addClass('item-disabled');
    $('.item-inc').removeClass('item-disabled');
    addLoader();
    row = $(this).closest('.item-row');
    id = $(this).closest('.item-row').data('pid');
    quant = $(this).find('.cart-quanity-val').text();
    $.post('/Cart/UpdateItem', { pid: id,type:0}, function (data) {
        row.find('.cart-quantity-val').html(data.quant);
        row.find('.item-total').html(PriceFormatted(data.quant * data.price));
        setTimeout(function () { closeLoader(data.s, data.t); $this.removeClass('item-disabled'); }, '200');        
        if (data.t == 1) $this.addClass('item-disabled');
    });
}
var cartItemIncrease = function () {
    $this = $(this);
    if ($(this).hasClass('item-disabled')) { return false; }
    $(this).addClass('item-disabled');
    $('.item-dec').removeClass('item-disabled');
    addLoader();
    row = $(this).closest('.item-row');
    id = $(this).closest('.item-row').data('pid');
    quant = $(this).find('.cart-quanity-val').text();

    $.post('/Cart/UpdateItem', { pid: id,type:1}, function (data) {
        row.find('.cart-quantity-val').html(data.quant);
        row.find('.item-total').html(PriceFormatted(data.quant * data.price));
        setTimeout(function () { closeLoader(data.s, data.t); $this.removeClass('item-disabled'); }, '200');
       
        if (data.t ==19) $this.addClass('item-disabled');
    });
}




/*=========================   Main   =============================*/

var mainConfiguration = function(){
    var width = $(window).width();
    var height = $(window).height();
    var input = $('<input type="hidden" id="screenWidth"  value="' + width + '">');
    var input1 = $('<input type="hidden" id="screenHeight"  value="' + height + '">');
    input.appendTo('body');
    input1.appendTo('body');

    var swidth = $('input[name="screenWidth"]').val();
    var mod = $('#modal-6');
    var mod1 = $('#modal-7');
    if (swidth < 768) {
        //mod.addClass('disabled-state');
        //mod1.addClass('disabled-state');
       
    }
    //$('img').filter(function () { return !$(this).hasClass("img-responsive")&&!$(this).closest('#listview').length; }).addClass('img-responsive');
}



var popupLivechat = function () {
    if ($('#livechat').length) {
        //alert('s');
        $('#livechat-wrap').show();

    }
    else {

        var wrapper = $("<div id='livechat-wrap'><img style='position:absolute;top:150px;left:140px' src='/Content/ajax-loaders/3011.gif' /></div>");
        $('body').append(wrapper);
        setTimeout(function () {
            $.get('/Consultant/GetChatRoom', function (html) {
                var $form = $("#livechat-feed");

                $form.removeData("validator");
                $form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($form);
                $('#livechat-wrap').css({ height: "auto" }).html(html);
            });

        }, 1000);
    }
}



var initSections = function () {
    $.getJSON("/StaticSection/GetSections", {}, function (data) {
        for (var i = 0; i < data.sections.length; i++) {
            $('#section_' + data.sections[i].SectionType).append(data.sections[i].Content);
        }
    });
}


var onToyBtnHover = function () {
    var t = $(this);
    var wraper = $('<div class="toy-tooltip" />');

}

var zoomedImg = function () {
    $('.folio-wrap li').hover(function () {
        $(this).find('span').addClass('folio-zoom-wrap');
    }, function () {
        $(this).find('span').removeClass('folio-zoom-wrap');
    }
    );
}

var zoomStyle = 'overflow: hidden; background-position: 0px 0px; text-align: center; background-color: rgb(255, 255, 255); width: 400px; height: 400px; float: left; background-size: 640px 480px; display: none; z-index: 100; border: 4px solid rgb(136, 136, 136); background-repeat: no-repeat; position: absolute; background-image:';
var prodimgList = function () {
    var id = $(this).data('id');
    var pid = $(this).data('pid');
    $('.prod-imglist li').css({ border: '1px solid #E4E4E4' });
    $(this).closest('li').css({ border: '1px solid rgb(189, 41, 41)' });
    $('#pid_' + pid + ' .prod-image img').attr({ src: $(this).attr('src') });
    //$('#pid_' + pid + ' .prod-image img').attr('height', '');
    //$('#pid_'+pid+' .prod-image img').attr('width', '');
}
var prodimgList1 = function () {
    var id = $(this).data('id');
    var pid = $(this).data('pid');


    $('.prod-imglist li').css({ border: '1px solid #E4E4E4' });
    $(this).closest('li').css({ border: '1px solid rgb(189, 41, 41)' });
    $('#pid_' + pid + ' .prod-image img').attr({ src: $(this).attr('src').replace('/200x150', '') });

    $('#pid_' + pid + ' .prod-image img').data('zoomImage', $(this).attr('src').replace('/200x150', ''));
    var isSmall = false;
    var tmpImg = new Image();
    tmpImg.src = $('.prod-image img').attr('src');
    tmpImg.onload = function () {
        if (XN.IsTablet) isSmall = true;
        if (this.width < 500 && !XN.IsTablet) isSmall = true;

        $('.zoomContainer').remove();
        $('#zoom_1').removeData('elevateZoom');
        if (!isSmall) $('#zoom_1').elevateZoom();
    };

    //console.log(w);

}
var postimgList = function () {
    var id = $(this).data('id');

    $('.post-imglist ul li').css({ border: '1px solid #E4E4E4' });
    $(this).closest('li').css({ border: '1px solid rgb(189, 41, 41)' });
    $('#post_main_img').attr({ src: $(this).data('src') });
    //$('#pid_' + pid + ' .prod-image img').attr('height', '');
    //$('#pid_'+pid+' .prod-image img').attr('width', '');
}


/*=============================  POPUP  ============================================== */




/*== JCarousel ==*/

function _init_slider(carousel) {
    $('#slider-nav a').bind('click', function () {
        var index = $(this).parent().find('a').index(this);
        carousel.scroll(index + 1);
        return false;
    });
};

function _active_slide(carousel, item, idx, state) {
    var index = idx - 1;
    $('#slider-nav a').removeClass('active');
    $('#slider-nav a').eq(index).addClass('active');
};

function _init_more_products(carousel) {
    $('.more-nav .next').bind('click', function () {
        carousel.next();
        return false;
    });

    $('.more-nav .prev').bind('click', function () {
        carousel.prev();
        return false;
    });
};
var disabled = function () { return false; };

var dropdownList2 = function (e) {
    $(this).closest('div.culture-info').find('.dropdown-list').show();
    $(this).closest('.dropdown-wrap2').addClass('drop2-active');
    e.stopPropagation();
}
var dropdownListItem2 = function (e) {

    var v = $(this).text();
    $wrap = $(this).closest('div.dropdown-wrap');
    $wrap.find('div.dropdown-list').hide();
    e.stopPropagation();

}
var dropdownList = function (e) {
    //alert('s');
    //e.stopPropagation();
    $(this).find('div.dropdown-list').show();
    e.stopPropagation();
}
var dropdownListItem = function (e) {

    var v = $(this).text();
    $wrap = $(this).closest('div.dropdown-wrap');
    $wrap.find('div.dropdown-list').hide();
    $wrap.find('div.dropdown-text').html(v);
    $wrap.find('input[type="hidden"]').attr('value', v.trim());
    //$(this).closest('label.dropdown-wrap').find('div.dropdown-text').html(v);

    e.stopPropagation();

}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};



/*=============================  Init Scripts  ============================================== */


$(function () {



    footNav();
    mainConfiguration();
    zoomedImg();  
    initSections();
    partialCartInit();
    summaryCartInit();
   
    
    
    $(document).on('click', '*[data-event-type="cart__decrease"]', cartItemDecrease);
    $(document).on('click', '*[data-event-type="cart__increase"]', cartItemIncrease);
    $(document).on('click', '*[data-event-type="cart__remove"]', removeFromCart);
    $(document).on('click', '*[data-event-type="cart__add"]', addToCart);
    //$(document).on('click', '*[data-event-type="cart__view"]', toTheCart);
    $(document).on('click', '*[data-event-type="cart__checkout"]', cartCheckout);
    //$(document).on('click', '.checkout-auth', authAttach);
    $(document).on('click', '#popup-livechat', popupLivechat);
    //$(document).on('click', '.dropdown-wrap', dropdownList);
    //$(document).on('click', '.dropdown-item', dropdownListItem);
    //$(document).on('click', '.culture-info .lg-i', dropdownList2);
    $(document).on('click', '.prod-img-wrapper .imglist-item', prodimgList);
    $(document).on('click', '.detail-item', prodimgList1);
    $(document).on('click', '.disabled-state', disabled);
    
    // $(document).on('click', '.dropdown-item', dropdownListItem);
    
    $(document).on("click", ".md-close", function (e) {
        XN.Inscreaser.Close();
        XN.Auth.CloseModal();
    });
    $(document).on("click", ".md-overlay", function (e) {
        if ($('.md-modal').hasClass('md-show')) $('.md-modal').removeClass('md-show');
    })
    $(document).on("keyup", "body", function (e) {
        if (e.keyCode == 13 || e.keyCode == 27) {
            XN.Inscreaser.Close();
            XN.Auth.CloseModal();
        }
        //if (fcont.css('marginTop') == '50px') fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
        if ($('.dropdown-list').is(':visible')) {
            $('.dropdown-wrap2').removeClass('drop2-active');
            $(".dropdown-list").hide();
        }

        //if ($('#popup-folio').hasClass('visible')) {
        //    $('#popup-folio').dialog('close');
        //    $('#popup-folio').removeClass('visible');
        //}
    });
    if ($('#ch-content').length) { $('.forg-btn').addClass('disabled-state'); $('.forg-btn').unbind('click'); return false; }



    //$('.cat-list li').hover(function () {
    //    $this = $(this);
    //    $this.find("i,a").css({ color: "#004CA3" });
    //    $this.find("a").css({ textDecoration: "none" });
    //}, function () {
    //    $this = $(this);
    //    $this.find("i,a").css({ color: "#4e89bf" });
    //    $this.find("a").css({ textDecoration: "none" });
    //});
    $('.prod-image').hover(function () {
        // $(this).find('.prod-details').show();
    }, function () { $(this).find('.prod-details').hide(); })


    $('#catsList li').removeClass('selected-cat');
    $('#catsList li').filter(function () {
        return $(this).data('catId') == getUrlParameter('catId');
    }).addClass('selected-cat');
    if (XN.WindowWidth <= 400) $('#catsList').addClass('hide-state');

    $('#bmenu li').removeClass('active');
    $('#bmenu li a').removeClass('current');
    $('#bmenu li').filter(function () {
        return $(this).find('a').attr('href') == window.location.pathname;
    }).find('a').addClass('current');

    $('.tmenu li').removeClass('active');
    $('.tmenu li a').removeClass('current');
    $('.tmenu li').filter(function () {
        return $(this).find('a').attr('href') == window.location.pathname;
    }).addClass('active').find('a').addClass('current');
    /* Pager
    ---------------------------------------------------------------------------------*/


});

