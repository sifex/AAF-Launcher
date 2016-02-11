<?php 
	
	$version = file_get_contents('version.txt');
	$disabled = false;
	$downloadURL = false;
	$callout = "";
	
	
	if(!glob("release/" . $version . "/*.zip")){
	    $disabled = true;
		$callout = '<span class="alert label">The current version is still under development. The download has been disabled at this time.</span><br /><br />';
	} else {
		$downloadURL =  "/release/latest/";
	}
	
	if(isset($_GET['update'])) {
		$callout = '<span class="alert label">You do not have an up to date version of the Mod Updater. Please download the updated version below.</span><br /><br />';
	}
	
	?>


<!doctype html>
<html class="no-js" lang="en">
<head>
<meta charset="utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1.0"/>
<title>AAF Updater</title>
<link rel="stylesheet" href="http://dhbhdrzi4tiry.cloudfront.net/cdn/sites/foundation.min.css">
<style>
.logo {
	background: url('http://i.imgur.com/iSdnxvR.png');
	background-size: 470px 194px; 
	height: 194px; 
	width: 470px;
	display: inline-block;
	margin-top: -50px;
}
h2 {
	margin-top: -30px;
}
</style>
</head>
<body>
 
<div class="top-bar">
<div class="top-bar-left">
<ul class="menu">
<li class="menu-text">AAF Updater</li>
</ul>
</div>
<div class="top-bar-right">
<ul class="menu"><!--
<li><a href="#">Three</a></li>
<li><a href="#">Four</a></li>
<li><a href="#">Five</a></li>
<li><a href="#">Six</a></li> -->
</ul>
</div>
</div>
 
<div class="callout large">
<div class="row column text-center">

<!-- <div class="logo"></div> -->

<h1>AAF Launcher</h1>
<h2><small>Changing the ARMA Modding World Forever.</small></h2>
<img src="http://i.imgur.com/4X73fR3.png" alt="">
<br /><br />
<p class="lead">Download and start updating right away! <a href="https://media.giphy.com/media/wErJXg1tIgHXG/giphy.gif">Just do it!</a></p>
<?php echo $callout; ?>
<a href="<?php echo ($downloadURL ? $downloadURL : '#'); ?>" class="button large <?php echo ($disabled ? 'disabled' : ''); ?>">Download Now</a>
<a href="#" class="button large hollow radius">Donate</a>
<div>
	<small>Current Version: <?php echo $version; ?></small>
</div>
</div>
</div><div class="row column text-right">
<ul class="menu"><!-- 
<li><a href="#">One</a></li>
<li><a href="#">Two</a></li>
<li><a href="#">Three</a></li>
<li><a href="#">Four</a></li> -->
</ul>
<small>Copyright &copy; 2016 Alex Sinnott</small>
</div>
<script src="https://code.jquery.com/jquery-2.1.4.min.js"></script>
<script src="http://dhbhdrzi4tiry.cloudfront.net/cdn/sites/foundation.js"></script>
<script>
      $(document).foundation();
    </script>
</body>
</html>
