/**
 * X-NOVA Widget Factory

 */
// Encoding: utf-8


var XN = XN || {}; // initialize global namespace
var mod, modc;

var ins_loader='<img class="ov-spin" src="/Content/ajax-loaders/horizont/tail-spin-gal.svg" />';
var ins_loader_black='<img class="ov-spin" src="/Content/ajax-loaders/horizont/tail-spin.svg" />';
var istablet = $('#screenWidth').val() < 768 ? "tablet" : "desktop";


$(function(){
    
    XN.Auth.startModal(0, null, null);
    // lazy loading - main-slider, prodlist
    var data = { type: istablet };
    var catid= $('.content-wrap').data('catid')!=undefined?0: $('#listview').data('catid');
    var data1 = { catId: catid, page: 1 };

    if ($('.s-slider').length) XN.AjaxRequest.MakeRequest('/Product/ProdsToSlider', { jsonData: JSON.stringify(data) }, '.s-slider');    
    if ($('#listview-target').length) XN.AjaxRequest.MakeRequest('/Product/ProdListPartial', { jsonData: JSON.stringify(data1) }, '#listview-target'); 
    // content image resize
    $('.content-wrap img').on('click', function () {
        var arr = [];        
        var index;
        var curr = $(this);
        var data = {};
        $('.content-wrap img').each(function (el) {
            if ($.inArray($(this).attr('src'), arr) === -1) arr.push($(this).attr('src'));
        });
        index = $.inArray(curr.attr('src'), arr);
        data = {
            el: index,
            pack:arr
        };
        //alert('index - ' + index + ', length - ' + arr.length + ' curr src - ' + curr.attr('src'));

        XN.Inscreaser.BuildModal('/Widget/Inscreaser', {jsonData:JSON.stringify(data)});
    });
    // folder image gallery
    $('*[data-ins-type="3"]').on('click', function () {
        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 3, folder: $(this).data('folder') });
    });
    // photogallery
    $('#photogallery-target .xn-listview-item').on('click', function () {
        
        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 2, id:$(this).data('id') });
    });
   // prodlist preview
    $('#listview-target .xn-listview-item  .xn-details').on('click', function () {
       
        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, id: $(this).data('id') });
    });
});
/*==========================================================================================*/
/*=====================================  Configuration ===============================*/
/*==========================================================================================*/


XN.IsMobile = $(window).width() < 400;  // -- проверка на мобильное устройство
XN.WindowHeight = $(window).height();
XN.AbsoluteUrl = document.URL;
XN.JavaEnabled = navigator.javaEnabled();
XN.Lang = navigator.language;



/*==========================================================================================*/
/*=====================================  Modal Auth/Register widget ===============================*/
/*==========================================================================================*/

XN.Auth = { // Widget - Modal Account Auth/Reg

    startModal: function (code, processType, username) {
       
        mod = $('#modal-7');
        modc = $('#modal-7').find('.md-content');
        if (processType == 1) {
            // Подверждение о успешной регистрации
            XN.BuildModal('#modal-7', '/Account/Mconf?username=' + username);
            return false;
        }
        else if (processType == 2) {
            // Сброс пароля
            XN.BuildModal('#modal-7', '/Account/Mset?userid=' + username + '&code=' + code);
            return false;
        }
        else { }

        XN.Auth.OpenEvents();

        // открытия мод окна и загрузка контента
        //   

        $('.feed-close').hover(function () {
            $(this).toggleClass("entypo-cancel");
            $(this).toggleClass("entypo-cancel-circled");
        }, function () {
            $(this).toggleClass("entypo-cancel");
            $(this).toggleClass("entypo-cancel-circled");
        });
    },
    CloseModal: function () {
        $('.md-modal').removeClass('md-show');
        //$('.md-modal').hide();
    },
    RemoveModal: function () {
        
        mod.removeClass('md-show');
        $('.md-auth .md-loader').show();
    },
    CloseEvent: function () {
        $('.md-close').on('click', function () {
           
            mod.removeClass('md-show');
        });
    },
    OpenEvents: function (form) {
        if (form == 1) {
            $('.md-content a[data-content-type="loginForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
            });
            $('.md-content a[data-content-type="registerForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mreg');
            });
            $('.md-content a[data-content-type="recoveryForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mrec');
            });
        }
        else {
            $('a[data-content-type="feedForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Feedback/Send');
            });
            $('a[data-content-type="loginForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
            });
            $('a[data-content-type="registerForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mreg');
            });
            $('a[data-content-type="recoveryForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mrec');
            });
        }
    },
    BuildModal: function (modalId, dataToLoad) {

        var modal = $(modalId);
        var loader = $('.md-auth .md-loader');
        var modalc = modal.find('.md-content');
        
        modalc.empty();
        XN.Auth.RemoveModal();
        $(".md-overlay").css({ background: 'rgba(255, 255, 255, 0.6)' });
        modal.addClass('md-show');
        modal.show();
        
        $.get(dataToLoad, function (data) {

            modal.find('.md-content').html(data);
            var form = modal.find('form');
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
            XN.Auth.CloseEvent();
            XN.Auth.OpenEvents(1);
            loader.hide();
        });


    }

}
/*==========================================================================================*/
/*=====================================  Modal PhotoGallery/Preview widget ===============================*/
/*==========================================================================================*/
XN.Inscreaser = { // Widget - Catalog/Gallery
    Init: function (type, el, index) {
        var input;

        var ins_modal = $('#modal-6');
        var ins_content = $('#modal-6 .md-content');
        var ins_scont = $('.xn-i-sphoto');
        var targetImage = $('#xn-i-main');

        var screenWidth = document.getElementById('screenWidth').getAttribute('value');
        var isMobile = screenWidth < 500;
        isMobile ? $('.md-modal').addClass('md-mobile') : "";
        var isGal = false;
        var prodMode = false;
        type == 2 ? isGal = true : "";
        type == 1 ? prodMode = true : "";

        /* конечная настройка виджета */
        $('.ov-spin').remove();
        /* ---------------------- */

        settings = {
            current: 0,
            list: $([]),
            wrapper: $([])
            //$('.xn-i-sphoto img')
        }


        if (type == 3) {

            /* Папка */
            for (var i = 0; i < el.length; i++) {
                var guiImg = $('<img alt="название фото" src="' + (this.Config().smallPhotoUrl + this.Utilities.FormatedUrl(el[i], this.Config().isCat)) + '" />');
                //alert(guiImg.attr('alt'));
                settings.wrapper = settings.wrapper.add(guiImg);

            }
        }
        else if (type == 2) {
            /* Фотогаллерея */
            for (i = 0; i < el.length; i++) {
                var guiImg = $('<img alt="title of image" data-id="' + el[i] + '" src="/ImageData/GetPhotoGalleryImage?id=' + el[i] + '&width=100&height=100&isGallery=false" />');
                settings.wrapper = settings.wrapper.add(guiImg);

            }
        }
        else if (type == 1) {
            /* Каталог */
            ins_modal.removeClass('xn-modal');
            ins_modal.addClass('md-cat');
            ins_modal.find('.xn-i-sphoto').removeAttr('style');
            ins_modal.find('.xn-i-sphoto').addClass('style');
            var rouble = $('<i class="fa fa-rouble"></i>');
            el.added ? $('.xn-cart-option').attr('data-event-type','cart__view').html('В корзину') : '';
            $('.xn-d-w a').attr('href', '/Product/Details/' + el.id).html(el.name);
            //$('.d-type td').html(data.name);
            $('.d-cat td').html(el.cat);
            $('.d-desc td').html(el.desc);
            $('.d-price td').html(el.price + ' <i class="fa fa-rouble"></i>');
            $('*[data-event-type="cart__add"]').attr('data-pid', el.id);
            //if(XN.IsMobile)$('.prod-link-details').attr('href', '/Product/Details/'+el.id);
            //$('.prod-link-details').attr('href', '/Product/Details/' + el.id);
            for (i = 0; i < el.si.length; i++) {
                var guiImg = this.Utilities.GetDataImg(el.si[i], false, 100, 100, el.id);
                //alert(sm.attr('src'));
                settings.wrapper = settings.wrapper.add(guiImg);

            }
            //$('body').append(simgs);

            $('.xn-h-w,.xn-d-w').fadeIn();
        }
        else {
            /* Контент  */
            var list = $('.content-wrap img').clone();
            var currList = ('.content-wrap img');
            var arr = [];
            settings.current = index;

            for (var i = 0; i < el.length; i++) {
                var guiImg = $('<img alt="название фото" src="' + (this.Config().smallPhotoUrl + this.Utilities.FormatedUrl(el[i], this.Config().isCat)) + '" />');
                settings.wrapper = settings.wrapper.add(guiImg);
            }
        }


        setTimeout(function () { $('.xn-i-sphoto').css({ visibility: 'visible' }); }, '700');


        ins_scont.html(settings.wrapper);
        settings.list = $('.xn-i-sphoto img');
        ins_scont.append('<div class="curr-border" />');

        settings.list.each(function (n) {
            $(this).removeClass('xn-i-current');
            $(this).attr('data-i-index', n);
            $(this).attr('data-pos', $(this).offset().left);
            if (n == settings.current) {
                $('.curr-border').css({ left: (n * $(this).outerWidth()) + 7 + "px" });
                //$('.curr-border').animate({ left: (n * $(this).outerWidth()) + 7 + "px" }, 300);
                $('.i-info').html($(this).attr('alt'));
                $(this).addClass("xn-i-current").css('opacity', '1');
                $('.i-counter').html(n + 1 + ' из ' + settings.list.length);
            }

        })
        .click(function () {
            ShowPhoto($(this).data('i-index'), true);
        });

        if (isGal) this.Utilities.LoadImg($('img[data-i-index="0"]').data('id'), 'gal');
        else if (prodMode) this.Utilities.LoadImg($('img[data-i-index="0"]').data('id'), 'prod');
        else this.Utilities.LoadImg(settings.current, 'content');

        $(this.Config().imgEl).css('cursor', 'pointer');

        // current image

        var currPos = this.Utilities.GetCurrentImg(settings.current);
        var elPos = $(ins_scont).find('img[data-i-index="' + settings.current + '"]').data('pos');

        settings.scontLeft = ins_scont.offset().left;
        // animate to current image
        var idx = currPos.index() + 1;
        var outer = currPos.outerWidth();
        ins_scont.animate({ left: (-outer * idx) + outer + "px" }, 300);

        // show large image
        function ShowPhoto(index, isPrev) {

            isPrev == null ? isPrev = false : "";
            settings.current = index;
            // load image event
            if (isGal) XN.Inscreaser.Utilities.LoadImg($('img[data-i-index="' + settings.current + '"]').data('id'), 'gal');
            else if (prodMode) XN.Inscreaser.Utilities.LoadImg($('img[data-i-index="' + settings.current + '"]').data('id'), 'prod');
            else XN.Inscreaser.Utilities.LoadImg(settings.current);
            // counter
            $('.i-counter').html(index + 1 + ' из ' + settings.list.length);

            // add current image and description
            settings.list.each(function (n) {

                $(this).removeClass('xn-i-current');
                $('.i-info').html(settings.list[index].alt);
                n == settings.current ? $(this).addClass("xn-i-current").css('opacity', '1') : $(this).css('opacity', '0.5');
            });

            isPrev == true ? "" : ins_scont.animate({ left: -outer * index + "px" }, 300);

            $('.curr-border').animate({ left: (index * outer) + 7 + "px" }, 300);
            $('.curr-border').data('offset', $('.curr-border').offset().left - $(window).scrollLeft());

        }

        $(this.Config().target).off();
        $(this.Config().ctlBprev).off();
        $(this.Config().ctlBnext).off();
        $('.xn-i-details__spoiler').off();
        $(this.Config().target).on('click', function (e) { ShowPhoto((settings.current + 1) % settings.list.length); });
        $(this.Config().ctlBprev).on('click', function () { settings.current == 0 ? ShowPhoto(0) : ShowPhoto((settings.current - 1) % settings.list.length); });
        $(this.Config().ctlBnext).on('click', function () { ShowPhoto((settings.current + 1) % settings.list.length); });
        $('.xn-i-details__spoiler').on('click', function () {

            $(this).siblings().slideToggle();

        });
        //XN.Inscreaser.Config({ isGal: true });
        //XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, folder: '/Content/qm/design' });
        //alert(settings.list.length);
    },
    Config: function (opts) {
        return $.extend(this.Default, opts || {});
    },

    Inscrease: function (type) {

    },
    BuildModal: function (dataUrl, opts) {

        this.Open(1);
        $.get(dataUrl, opts, function (data) {
            setTimeout(function () {
                $(XN.Inscreaser.Config().content).html(data);
                XN.Inscreaser.reEvent();
            }, '300');

            // <- reevent
        });
    },
    reEvent: function () {
        $('.md-close').on('click', function () {
            $(XN.Inscreaser.Config().content).html('');
            $(XN.Inscreaser.Config().modal).removeClass('md-show');
            $(XN.Inscreaser.Config().container).css('visibility', 'hidden');
        });


    },
    Open: function (type) {
        this.Close();
        $(this.Config().container).css('visibility', 'visible');
        $('body').attr('style', 'overflow:hidden');
        var preload = type == 0 ? ins_loader_black : ins_loader;
        $('.md-wrapper').append(preload);
        $(this.Config().modal).addClass('md-show');
    },
    Close: function () {
        $('body').removeAttr('style');
        // $(this.Config().modal).find("*").off();
        $(this.Config().content).html('');
        $(this.Config().modal).removeClass('md-show');
        $(this.Config().container).css('visibility', 'hidden');
        $('.xn-modal .md-content').removeAttr('style');

    },

    Utilities: {
        LoadImg: function (index, type) {
            var timer;
            function clearTimer() {
                if (timer) {
                    clearTimeout(timer);
                    timer = null;
                    $('.md-gif').remove();
                }
            }
            function paddingBottomProcents(width, height) {
                return Math.round((height * 100) / width);
            }

            $(XN.Inscreaser.Config().imgEl).hide();
            $('<img id="xn-i-beforesend" class="md-gif"  src="/Content/ajax-loaders/feed-load2.gif" width="31" height="31" />')
                .appendTo($(XN.Inscreaser.Config().imgEl)
                .closest('div'));

            var screenWidth = document.getElementById('screenWidth').getAttribute('value');
            var isMobile = screenWidth < 400;

            $('#xn-i-beforesend').hide();
            var img = new Image();

            $(XN.Inscreaser.Config().imgEl).removeClass('img-responsive');

            img.onload = function () {
                var loaded = document.getElementById(XN.Inscreaser.Config().imgEl.substring(1));
                var imgWrap = loaded.parentElement.parentElement;
                var modal = findParentBySelector(imgWrap, '.md-modal');

                // отображение картинки
                loaded.style.display = "";
                loaded.setAttribute('src', this.src);

                var wh = Math.max(modal.clientHeight - (loaded.height + 160)) / 2;

                var he = Math.max(modal.clientWidth - loaded.width) / 2;

                // ширина дефолтная для окна -- desktop,tablet
                if (!isMobile) {
                    //findParentBySelector(imgWrap, '.md-content').style.maxWidth = loaded.clientWidth + 326 + 'px';
                    //findParentBySelector(imgWrap, '.md-content').style.top = wh + 'px';
                }
                //loaded.setAttribute('class', 'img-responsive');

                /*    ПОСЛЕ РЕСПОНСИВА КАРТИНКИ    */

                // процент отступа для картинки
                var procent = paddingBottomProcents(img.width, img.height);

                loaded.parentElement.setAttribute('style', 'padding-bottom:' + procent + '%');

                var whMob = Math.max(modal.clientHeight - (loaded.height + 168));

                clearTimer();

            };

            if (type == 'post')
            { img.src = this.GetCurrentImg(index).attr('src').replace(/\&type=small/, "") + '&type=large'; }
            else if (type == 'gal') {
                img.src = '/ImageData/GetPhotoGalleryImage?id=' + index + '&width=750&height=500&isGallery=false';
            }
            else if (type == 'prod') {
                if (!isMobile) img.src = '/ImageData/GetProdImage?width=700&height=500&pimgid=' + index;
                else img.src = '/ImageData/GetProdImage?width=300&height=350&pimgid=' + index;

            }
            else {

                img.src = XN.Inscreaser.Config().largePhotoUrl + this.FormatedUrl(this.GetCurrentImg(index).attr('src'), true);

            }

            timer = setTimeout(function (theImg) {
                return function () {
                    $('#xn-i-beforesend').show();
                };
            }(img), 500);


        },
        GetDataImg: function (id, ispreview, width, height, pid) {
            return ispreview ?
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pid=' + id + '&width=' + width + '&height=' + height + '" />') :
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pimgid=' + id + '&width=' + width + '&height=' + height + '" />');
        },
        GetDataGalImg: function (id, isGal, width, height) {
            return ispreview ?
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pid=' + id + '&width=' + width + '&height=' + height + '" />') :
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pimgid=' + id + '&width=' + width + '&height=' + height + '" />');
        },
        FormatedUrl: function (url, isCat) {
            //alert(url);
            if (typeof url != 'undefined') {
                return isCat ? url.match(/\?path=\S+[a-zA-Z0-9_&%\.-]+/) : '?path=~' + url;
            }
        },
        GetCurrentImg: function (index) {
            return $('.xn-i-sphoto').find('img[data-i-index="' + index + '"]');
        },
        updateCartEvent: function (id, a) {

            if (a) {
                $('.prod_id-' + id).html('товар добавлен');
                $('.prod_id-' + id).removeClass('cart__add').addClass('cart__view');
            } else {
                $('.prod_id-' + id).html('Купить');
                $('.prod_id-' + id).removeClass('cart__view').addClass('cart__add');
            }
        }
    },
    Default: {
        modal: '#modal-6',
        overlay: '.md-overlay',
        content: '#modal-6 .md-content',
        container: '.md-container',
        scont: '.xn-i-sphoto',
        isCat: false,
        prodMode: false,
        isGal: false,
        isPost: false,
        imgEl: '#xn-i-main',
        ImagesDataUrl: '',
        largePhotoUrl: '/Widget/LoadMainPic',
        smallPhotoUrl: '/Widget/LoadSmallPic',
        ctlBnext: '.xn-i-mright',
        ctlBprev: '.xn-i-mleft',
        modalHeight: 600,
        modalWidth: 935,
        target: '#xn-i-main'


    }

}
/*==========================================================================================*/
/*=====================================  Lazy-Loading widget ===============================*/
/*==========================================================================================*/
XN.AjaxRequest = {
    Success: function (response) {
        setTimeout(function () {
            $(target).html(response);

        }, '500');
    },
    MakeRequest: function (url, data, target) {
       
            $.ajax({
                beforeSend: function () {
                    if (target == '#listview-target') $(target).html('<img class="ov-spin" src="/Content/ajax-loaders/horizont/tail-spin.svg" />');
                },
                url: url,
                data: data,
                success: function (response) {
                    setTimeout(function () {
                        $(target).html(response);
                        $('#listview-target .xn-listview-item  .xn-details').on('click', function () {

                            XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, id: $(this).data('id') });
                        });
                       

                        $('.page-link').on('click', function () {
                            var data = { catId: $(this).data('catid'), page: $(this).data('page') };
                            XN.AjaxRequest.MakeRequest('/Product/ProdListPartial', { jsonData: JSON.stringify(data) }, '#listview-target');
                        });

                    }, '500');
                }
            });
       
    }

}

function collectionHas(a, b) { //helper function (see below)
    for (var i = 0, len = a.length; i < len; i++) {
        if (a[i] == b) return true;
    }
   
    return false;
}
function findParentBySelector(elm, selector) {
    var all = document.querySelectorAll(selector);
    var cur = elm.parentNode;
    while (cur && !collectionHas(all, cur)) { //keep going up until you find a match
        cur = cur.parentNode; //go up
    }
    return cur; //will return null if not found
}










