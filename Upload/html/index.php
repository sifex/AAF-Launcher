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
	<link href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,300' rel='stylesheet' type='text/css'>
	<style type="text/css" media="screen">

		#draggable {
			height: 26px;
			position: fixed;
			width: 100%;
		}
		#minimise, #close {
			height: 26px;
			width: 34px;
			position: fixed;
		}
		#startGame {
			width: 150px;
			height: 56px;
			position: absolute;
			top: 379px;
			left: 736px;
			background: #2991ed;
			color: #FFF;
			font-size: 18px;
			font-family: 'Source Sans Pro', sans-serif;
			text-shadow: 0px 5px 6px rgba(0, 0, 0, 0.73);
			font-weight: 300;
		}
		#close {
			background: url('http://i.imgur.com/NBY6haG.png');
			left: 866px;
		}
		#close:hover {
			background: url('http://i.imgur.com/VvXKhGC.png');
		}
		#minimise {
			background: url('http://i.imgur.com/smlvZuR.png');
			left: 832px;
		}
		#minimise:hover {
			background: url('http://i.imgur.com/LQOLaD9.png');
		}
		
		#loadingBar {
			width: 710px;
			height: 31px;
			background: rgb(9,15,32);
			position: absolute;
			top: 404px;
			left: 14px;
		}
		#loadingBar #progress {
			width: 0;
			height: 100%;
			background: #1E90FF;
		}
		*:not(.small-11) {
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
		#configButton.disabled {
			color: #acacac;
			cursor: default;
		}
		#configButton.disabled:hover {
			color: #acacac;
			cursor: default;
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
	<div id="draggable"></div>
	<div id="close"></div>
	<div id="minimise"></div>
	<div id="loadingBar">
		<div id="progress"></div>
	</div>
	<button id="startGame" class="button">Begin Update</button>
	<a onClick="window.external.openURL('http://australianarmedforces.org/')" href="#">
		<div class="logo"></div>
	</a>
	<span>
		Copyright &copy; 2016 Australian Armed Forces 
		<br /> 
		AAF Launcher – Version <?php echo file_get_contents('../version.txt'); ?>
		<br />
		<a href="ts3server://ts.australianarmedforces.org?port=9987">Teamspeak</a>
		   |   
		<a style="color: #cd4c2e" onClick="window.external.openURL('http://development.australianarmedforces.org/secure/CreateIssue!default.jspa')" href="#">Report an Issue</a>
		<br />
		<a id="configButton" data-open="exampleModal">Config</a>
	</span>
	<div id="owl-demo" class="owl-carousel">
		<div class="banner one"></div>
		<div class="banner two"></div>
		<div class="banner three"></div>
	</div>

	<div class="updateText">
		<p class="status"></p><p class="file"></p>
	</div>

	<div class="reveal" id="exampleModal" data-reveal>
		<form id="installForm">
	        <div class="row">
	          <div class="medium-12 columns">
	            <label>Install Directory</label>
	              <input type="text" class="small-11 columns" placeholder="" id="filePath">
				  <a id="installDirSelect" class="small-1 columns text-center button" href="#" onClick="window.external.ChooseFolder()">...</a>
	            
	          </div>
	          
	        </div>
	        <div class="row">
		        <div class="small-12 columns">
				  	<input type="submit" value="Submit" id="installDirSubmit" class="small-12 columns text-center button success" />
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
			
		var done = false;
		
		function get(url) {
			$.get(url, function( data ) {
				return data;
			});
		}
		
		$("#draggable").mousedown(function() {
			window.external.drag();
		});
		
		$(function() {
			$.get("http://scarlet.australianarmedforces.org/api/user/install/<?php echo $_GET['scarletKey']; ?>/", function(data){
				$("#filePath").val(data);
			});
		});
		
		$("#installForm").submit(function(e) {
			e.preventDefault();
			$.post("http://scarlet.australianarmedforces.org/api/user/install/<?php echo $_GET['scarletKey']; ?>/", {installDir: $("#filePath").val()}).done(function() {
					$('#exampleModal').foundation('close');
					window.external.refreshStatus();
				});
		});
		
		$("#close").click(function() {
			window.external.closeBtn_Click();
		});
		
		$("#minimise").click(function() {
			window.external.minimizeBtn_Click();
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
				$(".file").css("color", "rgb(" + color + ")");
			}
		}
		
		function updateProgress(progress, color) {
			$("#progress").width(function() {
				return progress * $("#loadingBar").width();
			});
			if(typeof color != 'undefined') {			
				$("#progress").css("background-color", "rgb(" + color + ")");
			}
		}
		
		function disableConfig() {
			$("#configButton").removeAttr('data-open');
			$("#configButton").attr('class', 'disabled');
		} /*
		
		function enableConfig() {
			$("#configButton").attr('data-open', "exampleModal");
			$("#configButton").removeAttr('class');
		} */
		
		function fillPath(path) {
			$("#filePath").val(path);
		}
		
		$("#startGame").click(function() {
			if(done == false) {
				$(this).attr('class', 'button disabled');
				window.external.update_Click();
			}
			else {
				window.external.strtGameBtn_Click();	
			}
		});	
		
		function completed() {
			$("#startGame").css('background', 'rgb(100, 206,63)');
			$("#startGame").attr('class', 'button');
			$("#startGame").html("Start Game");
			
			done = true;
		}


		
	</script>
	
</body>
</html>