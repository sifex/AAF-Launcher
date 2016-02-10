<!DOCTYPE html>
<html lang="en">
<head>
	<meta http-equiv="cache-control" content="no-cache">
	<meta name="viewport" content="user-scalable=no" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge"> 
	<meta charset="UTF-8">
	<title>Australian Armed Forces</title>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js" charset="utf-8"></script>
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/foundation/6.1.1/foundation.css">
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/owl-carousel/1.3.3/owl.carousel.min.css" type="text/css" media="screen" charset="utf-8">
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/owl-carousel/1.3.3/owl.transitions.min.css" type="text/css" media="screen" charset="utf-8">
	<style type="text/css" media="screen">
		* {
			-webkit-user-select: none; /* Chrome/Safari */        
			-moz-user-select: none; /* Firefox */
			-ms-user-select: none; /* IE10+ */
			
			/* Rules below not implemented in browsers yet */
			-o-user-select: none;
			user-select: none;
		}
		[type='text'].small-11 {
			width: 91.66667%;
		}
		span:hover, p:hover {
			cursor: default;
		}
		html, body {
			overflow: hidden;
			width: 900px;
			height: 450px;
		}
		body {
			background-image: url('/html/img/bg.png');
			background-repeat: no-repeat;
			width: 900px;
			height: 450px;
		}
		.logo {
			background-image: url('/html/img/logo.png');
			height:88px;
			width: 253px;
			margin: 35px 40px 20px;
		}
		.banner {
			width: 900px;
			height: 223px;
			-webkit-background-size: cover;
			-moz-background-size: cover;
			-o-background-size: cover;
			background-size: cover;
			background-position: center center;
		}
		.banner.one {
			background-image: url('http://s3.foxfilm.com/foxmovies/production/films/103/images/gallery/deadpool1-gallery-image.jpg');
		}
		.banner.two {
			background-image: url('http://cdn2.gamefront.com/wp-content/uploads/2013/10/ARMA-III-4K.jpg');
		}
		.banner.three {
			background-image: url('https://www.bistudio.com/assets/legacy/images/stories/arma3/screenshots/Arma3_screenshot_1202_22.jpg');
		}
		span {
			color: #323c4c;
			position: absolute;
			top: 30px;
			left: 390px;
			font-size: 11px;
			text-align: right;
			width: 500px;
		}
		.updateText {
			margin: 10px 0 0 15px;
		}
		.status, .file {
			display: inline-block;
			font-size: 11px;
			font-family: 'Tahoma', sans-serif;
		}
		.status {
			color: #2199e8;
			margin-right: 10px;
		}
		.file {
			color: rgba(74, 105, 136, 1);
		}
		.reveal-overlay {
			overflow: hidden;
		}
	</style>
</head>
<body>
<a href="http://australianarmedforces.org/"><div class="logo"></div></a>
<span>Copyright &copy; 2016 Australian Armed Forces <br> 
AAF Launcher – Version <?php echo file_get_contents('../version.txt'); ?> <br/>
<a href="ts3server://ts.australianarmedforces.org?port=9987">Teamspeak</a>   |   <a style="color: #cd4c2e" href="http://development.australianarmedforces.org/projects/SCARLET/issues">Report a bloody Issue</a>
<br />
<a href="#config" data-open="exampleModal">Config</a></span>
<div id="owl-demo" class="owl-carousel">
	<div class="banner one"></div>
	<div class="banner two"></div>
	<div class="banner three"></div>
</div>
<div class="updateText">
	<p class="status"></p><p class="file"></p>
</div>

<div class="reveal" id="exampleModal" data-reveal>
	<form>
        <div class="row">
          <div class="medium-12 columns">
            <label>Install Directory</label>
              <input type="text" class="small-11 columns" placeholder="" id="filePath">
			  <a id="installDirSelect" class="small-1 columns text-center button" href="#">...</a>
            
          </div>
          
        </div>
          <div class="row">
	          <div class="small-12 columns">
			  	<a id="installDirSubmit" data-close class="small-12 columns text-center button success" href="#">Submit</a>
	          </div>
          </div>
      </form>
</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.0/jquery.min.js" charset="utf-8"></script>
<script src="/html/js/owl.carousel.min.js" charset="utf-8"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/foundation/6.1.2/foundation.js" charset="utf-8"></script>
<script>
	$(document).ready(function() {
		$("#owl-demo").owlCarousel({
 
			navigation : false, // Show next and prev buttons
			singleItem: true,
			transitionStyle : "fade",
			autoPlay: 15000,
			stopOnHover : false,
			dragBeforeAnimFinish : false,
			mouseDrag : false,
			touchDrag : false
 
      // "singleItem:true" is a shortcut for:
      // items : 1, 
      // itemsDesktop : false,
      // itemsDesktopSmall : false,
      // itemsTablet: false,
      // itemsMobile : false
 
  });
	});
	
	$(document).foundation();
</script>

	<script>
		
		function get(url) {
			$.get(url, function( data ) {
				return data;
			});
		}
		
		$(function() {
			$.get("http://scarlet.australianarmedforces.org/api/user/install/42de65d777b8bcb2a2cf5396c87f8508/", function(data){
				$("#filePath").val(data);
			});
		});
		
		$("#installDirSubmit").click(function() {
			$.post("http://scarlet.australianarmedforces.org/api/user/install/42de65d777b8bcb2a2cf5396c87f8508/", {installDir: $("#filePath").val()});
		});
		
		function updateStatus(string, color) {
			$(".status").html(string);
			if(typeof color != 'undefined') {	
				$(".status").css("color", "rgb(" + color + ")");
			}
		}
		
		function updateFile(string, color) {
			$(".file").html(string);
			if(typeof color != 'undefined') {			
				$(".status").css("color", "rgb(" + color + ")");
			}
		}
	</script>
</body>
</html>