﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(function () {
    var placeholderElement = $('#gallery-modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {

            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            var UrlRedirect = newBody.data('url');
            if (isValid) {

                placeholderElement.find('.modal').modal('hide');
                window.location.href = UrlRedirect;
            }
        });
    });
});

$(function () {
    var imageplaceholderElement = $('#image-modal-placeholder');

    //When a button which as the attribute data-toggle = ajax-modal-image is clicked, trigger a function
    $('button[data-toggle="ajax-modal-image"]').click(function (event) {
        event.stopPropagation();
        //get the value of the attribute data-url  
        var url = $(this).data('url');
        //load the result of the request (get url) => a partial view containing the modal to show
        $.get(url).done(function (data) {
            //place the loaded content in the place holder and "open it" (show it)
            imageplaceholderElement.html(data);
            imageplaceholderElement.find('.modal').modal('show');
        });
    });

    //Actions triggered when the "save" button from the form is clicked
    imageplaceholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        //get routing url as defined in the form 
        var actionUrl = form.attr('action');
        var dataToSend = new FormData(form.get(0));
        //Ajax request that is triggered when the save button is clicked
        $.ajax({
            url: actionUrl,
            method: 'post',
            data: dataToSend,
            processData: false,
            contentType: false
            //function that is trigger 
        }).done(function (data) {
            // data is the rendered in _ImageModalPartial
            var newBody = $('.modal-body', data);
            // replace modal contents with new form
            imageplaceholderElement.find('.modal-body').replaceWith(newBody);
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            // get url for the redirection: it's the value of data-url from the element newBody
            var UrlRedirect = newBody.data('url');
            if (isValid) {
                //Close the de modal (hide it)
                imageplaceholderElement.find('.modal').modal('hide');
                //redirct to the UrlRedirct value => a refresh of the page with route to the value of the opened gallery
                window.location.href = UrlRedirect;
            }
        });
    });
});

$(function () {
    var viewImagePlaceholderElement = $('#viewimage-modal-placeholder');

    $('div[data-toggle="ajax-modal-viewimage"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            viewImagePlaceholderElement.html(data);
            viewImagePlaceholderElement.find('.modal').modal('show');
        });
    });

    viewImagePlaceholderElement.on('click', '[data-toggle="ajax-modal-navigateImage"]', function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {

            var newBody = $('.modal-body', data);
            viewImagePlaceholderElement.find('.modal-body').replaceWith(newBody);
            viewImagePlaceholderElement.find('.modal').modal('show');
        });

    });
});

//$(function () {
//    var viewImagePlaceholderElement = $('#viewimage-modal-placeholder');

//    $('div[data-toggle="ajax-modal-viewimage"]').click(function (event) {
//        var url = $(this).data('url');
//        $.get(url).done(function (data) {
//            viewImagePlaceholderElement.html(data);
//            viewImagePlaceholderElement.find('.modal').modal('show');
//        });
//    });

//    viewImagePlaceholderElement.on('click', '[data-toggle="ajax-modal-navigateImage"]', function (event) {
//        var url = $(this).data('url');
//        $.get(url).done(function (data) {

//            var newBody = $('.modal-body', data);
//            viewImagePlaceholderElement.find('.modal-body').replaceWith(newBody);
//            viewImagePlaceholderElement.find('.modal').modal('show');
//        });

//    });
//});



$('input[data-toggle="ajax-modal-image"]').click(function (e) {
    // Do something
    e.stopPropagation();
});
//// Write your JavaScript code.
