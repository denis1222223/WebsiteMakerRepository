$(function () {
    $('.img-radio').click(function (evente) {
        $('[name = ' + $(this).attr('name') + ']').not(this).removeClass('active')
    		.siblings('input').prop('checked', false)
            .siblings('.img-radio').css('box-shadow', 'none');
        $(this).addClass('active')
            .siblings('input').prop('checked', true)
    		.siblings('.img-radio').css('box-shadow', '0 0 10px rgba(0,0,0,0.5)');
    });
});

$(function () {
    $('.check-input').keypress(function (event) {
        if (!((event.which >= 65 && event.which <= 90) ||
            (event.which >= 97 && event.which <= 122) ||
            (event.which >= 48 && event.which <= 57) ||
            (event.which == 32))) {
            event.preventDefault();
        }
    });
});