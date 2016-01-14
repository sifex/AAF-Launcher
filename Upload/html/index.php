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
		html, body {
			overflow: hidden;
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
			background-image: url('http://espritcine.fr/wp-content/uploads/2016/01/12484692_10153846773349723_113997982607873822_o.jpg');
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
	</style>
</head>
<body>
<div class="logo"></div>
<span>Copyright &copy; 2016 Alex Sinnott <br> 
AAF Launcher – Version <?php echo file_get_contents('../version.txt'); ?> <br/>
<a href="ts3server://ts.australianarmedforces.org?port=9987">Teamspeak</a></span>
<div id="owl-demo" class="owl-carousel">
	<div class="banner one"></div>
	<div class="banner two"></div>
</div>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0-alpha1/jquery.min.js" charset="utf-8"></script>
<script src="/html/js/owl.carousel.min.js" charset="utf-8"></script>

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
</script>
</body>
</html>