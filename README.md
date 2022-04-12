# Convert Files

Convert Files is a basic C# console application that receive a CSV file and convert it to JSON or XML

#### Some assumptions are made about the CSV file

- The headings should form keys in the XML or JSON output
- Underscores in header should be used to group headings
  - Ex: 
  
    > name,address_line1,address_line2
    > 
    > Dave,Street,Town
    
    Should convert to JSON like this:

        {
            name: Dave,
            address: {
                line1: Street,
                line2: Town
            }
        }

#### Future requirements taken into account

- Possibility to convert to or from other file type or between XML and JSON
- Possibility to load original data from a source other than a disk file