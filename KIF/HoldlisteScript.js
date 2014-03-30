
function Afdeling() 
{
    return $('#LeftMenu').find('.rmFirst').find('.rmText').text();
}

$(document).ready(function () {
    $('#iframe').attr('src', 'http://kif.noerup-sostack.dk/holdliste.aspx?afdeling=' + Afdeling());
});
