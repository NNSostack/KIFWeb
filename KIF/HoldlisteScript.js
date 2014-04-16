
function Afdeling() 
{
    return $('#LeftMenu').find('.rmFirst').find('.rmText').text();
}

$(document).ready(function () {
    $('#iframe').attr('src', 'http://noerup-sostack.dk/kif/holdliste.aspx?afdeling=' + Afdeling());
});
