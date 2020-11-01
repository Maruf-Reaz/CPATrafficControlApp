/**
*  * Returns a random integer between min and max for random background image
 Using Math.round() will give you a non-uniform distribution!
**/
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
var img = getRandomInt(1, 12);
var extension = '.jpg';
var url = '/asset/login/img/' + img + extension;
$(document).ready(function () {
    setFocus()
});
$(window).on('load', function () {
    $.backstretch(url);
    if ($('#password').length > 0) {
        isChromeAndAutocomplete();
    }
});