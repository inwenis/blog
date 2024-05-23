# hide this somewhere

ls -Recurse -File `
| % {cat $_.FullName} `
| sls -pattern 'https?:.*?(?=>",|"|",|\|)' -all `
| % { $_.Matches[0].value } `
| sort `
| select -Unique `
| % { $_.Replace("\/", "/")} > out.txt
