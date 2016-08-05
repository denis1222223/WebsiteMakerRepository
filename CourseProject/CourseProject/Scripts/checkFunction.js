$(function () {
    $('.img-radio').click(function (e) {       
        $('[name = '+ $(this).attr('name') + ']').not(this).removeClass('active')
    		.siblings('input').prop('checked', false)
            .siblings('.img-radio').css('box-shadow', 'none');
        $(this).addClass($(this).attr('name'))
            .siblings('input').prop('checked', true)
    		.siblings('.img-radio').css('box-shadow', '0 0 10px rgba(0,0,0,0.5)');
    });
});