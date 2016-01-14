<?php
	
	$modDir = "./@Mods_AAF";

	function getDirMap($dir) {
		$root = $dir;

		$iter = new RecursiveIteratorIterator(
		    new RecursiveDirectoryIterator($root, RecursiveDirectoryIterator::SKIP_DOTS),
		    RecursiveIteratorIterator::SELF_FIRST,
		    RecursiveIteratorIterator::CATCH_GET_CHILD // Ignore "Permission denied"
		);

		$paths = array($root);
		foreach ($iter as $path => $dir) {
		    if ($dir->isDir()) {
		        $paths[] = $path;
		    }
		}

		return ($paths);
	}

	function getEntireDirMap($dir) {
		$return = array();
		foreach(getDirMap($dir) as $folder) {
			$scanned_directory = array_diff(scandir($folder), array('..', '.', '.DS_Store'));
			foreach($scanned_directory as $file) {
				$return[substr(($folder . "/" . $file), 1)] = (explode(" ", shell_exec('md5sum -b ' . escapeshellarg($folder . "/" . $file)))[0]);
			}
		}
		return ( $return );
	}

	function generateUpdates($dir) {
		$xml ='';
		$xml .= '<theupdates>';
		$xml .= '<modDir><name>' . substr($dir,2) . '</name></modDir>';
		foreach(getEntireDirMap($dir) as $key => $comb) {
			if(!strpos($key,'.')) {
				$xml .= '<directory><name>' . $key . '</name></directory>';
			} else {
				$xml .= '<file><name>' . $key . '</name><hash>' . $comb . '</hash></file>';
			}
		}
		$xml .=  '</theupdates>';
		return $xml;
	}
	
	function writeToRepoXML($string, $file){
		$xml = new SimpleXMLElement($string);
		
		file_put_contents($file,$xml->saveXML());
	}

	function formatXML($xml) {
		header("Content-type: text/xml");
		$dom = new DOMDocument;
		$dom->preserveWhiteSpace = FALSE;
		$dom->loadXML($xml);
		$dom->formatOutput = TRUE;
		return $dom->saveXml();
	}
	
	if($_GET['modAuth'] == "24ERpTrR2LDH9xj2MZAwZMY2mkde") {
		file_put_contents("./repo.xml", generateUpdates($modDir));
		echo formatXML(generateUpdates($modDir));
	} else {
		header("Location: http://" . $_SERVER['HTTP_HOST'] . "/html/");
	}

?>