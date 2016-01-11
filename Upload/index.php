<?php
	
	$modDir = "./Mods_AF";

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
				$return[substr(($folder . "/" . $file), 1)] = (md5_file($folder . "/" . $file));
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

	function formatXML($xml) {
		header("Content-type: text/xml");
		$dom = new DOMDocument;
		$dom->preserveWhiteSpace = FALSE;
		$dom->loadXML($xml);
		$dom->formatOutput = TRUE;
		return $dom->saveXml();
	}
	
	echo formatXML(generateUpdates($modDir));

?>