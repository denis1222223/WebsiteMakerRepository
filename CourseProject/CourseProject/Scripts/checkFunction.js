$(function () {
    $('.img-radio').click(function (e) {
        $('.img-radio').not(this).removeClass('active')
    		.siblings('input').prop('checked', false)
            .siblings('.img-radio').css('opacity', '0.5');
        $(this).addClass('active')
            .siblings('input').prop('checked', true)
    		.siblings('.img-radio').css('opacity', '1');
    });
});