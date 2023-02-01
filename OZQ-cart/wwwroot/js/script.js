$("document").ready(function () {
 
  window.products = {};
  $.ajax({
    type: "GET",
    url: "./api/Items",
    contentType: "application/json",
    // data: JSON.stringify({
    //     "email": "test@example.com",
    //     "password": "strin123#D",
    //     "rememberMe": true
    // }),
    success: function (data, status, xhr) {
      var items = data;
      window.products = items;
      console.log(items);

      //retrieve token from the response and store it
      var container = document.getElementById("product-container") || null;
      var carouselContainer =
        document.getElementsByClassName("carousel-container") || null;
      if (container || carouselContainer) {
        for (var i = 0; i < items.length; i++) {
          var itemElement = `<div class="col-sm-3">
                                      <div class="thumb-wrapper">
                                          <span class="wish-icon"><i class="fa fa-heart-o"></i></span>
                                          <div class="img-box">
                                              <img src="${
                                                items[i].image
                                              }" class="img-fluid product-img" alt="Watch">
                                          </div>
                                          <div class="thumb-content">
                                              <h4>${items[i].name}</h4>
                                              <p class="item-price"><strike>Rs. ${
                                                items[i].price * 1.2
                                              }</strike> <span>Rs. ${
            items[i].price
          }</span></p>
                                              <div class="star-rating">
                                                  <ul class="list-inline">
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star-o"></i></li>
                                                  </ul>
                                              </div>
                                              <button onclick="addSingleCartItem(${
                                                items[i].id
                                              })" class="btn btn-primary">Add to Cart</button>
                                          </div>
                                      </div>
                                  </div>`;

          try {
            if (container) {
              container.innerHTML += itemElement;
            }

            if (carouselContainer) {
              if (i < 4) {
                carouselContainer[0].innerHTML += itemElement;
              } else if (i < 8) {
                carouselContainer[1].innerHTML += itemElement;
              } else if (i < 12) {
                carouselContainer[2].innerHTML += itemElement;
              }
            }
          } catch (e) {
            console.log(e);
          }
        }
      }
    },
    error: function (data, xhr, status, error) {
      console.log(data.responseJSON);
    },
  });

  $.ajax({
    type: "GET",
    url: "./api/Authentication/currentuser",

    success: function (data, status, xhr) {
      //retrieve token from the response and store it

      // Extract the cookies from the response headers
      var cookies = xhr.getResponseHeader("set-cookie");
      console.log(data);

      window.logged = true;
      window.userId = data.id;
      window.userEmail = data.email;

      if (window.userEmail) {
        $.ajax({
          type: "GET",
          url: "./api/Customers/email/" + window.userEmail,
          contentType: "application/json",
          success: function (data, status, xhr) {
            //retrieve token from the response and store it
            console.log(data);
            window.customerId = data.id;
            window.customerName = data.name;
            window.address = data.address;
          },
          error: function (data, xhr, status, error) {
            console.log(data.responseJSON);
          },
        });
      }

      document.getElementById("logged-in-nav").hidden = false;
      document.getElementById("logged-out-nav").hidden = true;
      // Set the cookies in the browser
      // document.cookie = cookies;
    },
    error: function (xhr, status, error) {
      window.logged = false;

      document.getElementById("logged-in-nav").hidden = true;
      document.getElementById("logged-out-nav").hidden = false;

      console.log("error is there" + error);
    },
  });

  $("#logout").click(function () {
    $.ajax({
      type: "POST",
      url: "./api/Authentication/logout",

      // }),
      success: function (data, status, xhr) {
        alert("Logout successful");
        window.location.href = "index.html";
      },
      error: function (xhr, status, error) {
        alert("Logout failed");
      },
    });
  });
});

function searchItems() {
  event.preventDefault();
  console.log("clicked");
  var query = document.getElementById("search-query").value;
  console.log(query);
  var searchUrl = "./api/Items/name/" + query;

  $.ajax({
    type: "GET",
    url: searchUrl,
    dataType: "json",
    success: function (data) {
      if (data.length > 0) {
        var searchResults = "";
        for (var i = 0; i < data.length; i++) {
          var item = data[i];
          var itemElement = `<div class="col-4">
                                      <div class="thumb-wrapper">
                                          <span class="wish-icon"><i class="fa fa-heart-o"></i></span>
                                          <div class="img-box">
                                              <img src="${
                                                item.image
                                              }" class="img-fluid product-img" alt="Watch">
                                          </div>
                                          <div class="thumb-content">
                                              <h4>${item.name}</h4>
                                              <p class="item-price"><strike>Rs. ${
                                                item.price * 1.2
                                              }</strike> <span>Rs. ${
            item.price
          }</span></p>
                                              <div class="star-rating">
                                                  <ul class="list-inline">
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star"></i></li>
                                                      <li class="list-inline-item"><i class="fa fa-star-o"></i></li>
                                                  </ul>
                                              </div>
                                              <button onclick="addSingleCartItem(${
                                                item.id
                                              })" class="btn btn-primary">Add to Cart</button>
                                          </div>
                                      </div>
                                  </div>`;
          searchResults += itemElement;
        }
        $("#search-results-modal-body").html(searchResults);
        $("#search-results-modal").modal("show");
      } else {
        alert("No results found for '" + query + "'");
      }
    },
    error: function (error) {
      console.log(error);
      alert("An error occurred while searching. Please try again later.");
    },
  });
}
