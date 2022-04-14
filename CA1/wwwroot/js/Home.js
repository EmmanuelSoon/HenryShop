window.onload = function () {

    let add = document.getElementsByClassName("btn-outline-primary");
    let addwishlists = document.getElementsByClassName("fa-heart");

    for (let i = 0; i < add.length; i++) {
        add[i].addEventListener('click', Addclick);
    }
    for (let i = 0; i < addwishlists.length; i++) {
        addwishlists[i].addEventListener('click', Addlist);
    }

    function Addclick(event) {
        AddToCart(event.target.value);
    }
    function Addlist(event) {
        AddToWishList(event.target.value);
    }

}



function AddToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Search/AddToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {

                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    $('body').append(
                        '<div></div><div id="myModal" class="modal fade" role="dialog">' +
                        '  <div class="modal-dialog">' +
                        '    <div class="modal-content">' +
                        '      <div class="modal-header">' +
                        '        <h3 class="modal-title">Success!</h3>' +
                        '        <button type = "button" class= "close" data-dismiss="modal">&times;</button>' +
                        '      </div>' +
                        '      <div class="modal-body">' +
                        '           <p>' + data.name + ' Added to cart' +
                        '</p>' +
                        '      </div>' +
                        '      <div class="modal-footer">' +
                        '        <button type="button" class="btn btn-default" data-dismiss="modal">Continue Shopping</button>' +
                        '      </div>' +
                        '    </div>' +
                        '  </div>' +
                        '</div>');
                    $('#myModal').modal('show');
                    
                    //alert(data.name + " Added to cart");
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}

function AddToWishList(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Wishlist/AddToWishList");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {
                    alert(data.name + " Added to your wishlist");
                }
                else if (data.status == "existed") {
                    alert(data.name + " Already existed in your wishlist");
                }
                else if (data.status == "needlogin") {
                    alert("PLease Login to add product in the wishlist");
                    window.location.href = '/Login/Index/';
                }
            }
        }
    }

    let req = { "Id": id }
    xhr.send(JSON.stringify(req));
}