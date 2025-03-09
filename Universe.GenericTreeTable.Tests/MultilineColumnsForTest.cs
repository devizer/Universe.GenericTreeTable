using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Universe.GenericTreeTable.Tests;

public class MultilineColumnsForTest
{
	private static Lazy<List<string[]>> _Columns;
	public static List<string[]> Columns => _Columns.Value;

	static MultilineColumnsForTest()
	{
		_Columns = new Lazy<List<string[]>>(GetColumns, LazyThreadSafetyMode.ExecutionAndPublication);
	}

	private static List<string[]> GetColumns()
	{
		var arr = RawColumns.Split(' ', '\r', '\n')
			.Select(x => x.Trim())
			.Where(x => x.Length > 0)
			.ToArray();

		var textInfo = CultureInfo.InvariantCulture.TextInfo;
		var ret = arr.Select(x => x.Split('_').Select(x => textInfo.ToTitleCase(x)).ToArray()).ToList();
		return ret;
	}

	public static readonly string RawColumns = @"
lob_fetch_in_pages                   
lob_fetch_in_bytes                   
lob_orphan_create_count              
lob_orphan_insert_count              

leaf_insert_count                    
leaf_delete_count                    
leaf_update_count                    
leaf_ghost_count                     
nonleaf_insert_count                 
nonleaf_delete_count                 
nonleaf_update_count                 
leaf_allocation_count                
nonleaf_allocation_count             
leaf_page_merge_count                
nonleaf_page_merge_count             
range_scan_count                     
singleton_lookup_count               
forwarded_fetch_count                
row_overflow_fetch_in_pages          
row_overflow_fetch_in_bytes          
column_value_push_off_row_count      
column_value_pull_in_row_count       
row_lock_count                       
row_lock_wait_count                  
row_lock_wait_in_ms                  
page_lock_count                      
page_lock_wait_count                 
page_lock_wait_in_ms                 
index_lock_promotion_attempt_count   
index_lock_promotion_count           
page_latch_wait_count                
page_latch_wait_in_ms                
page_io_latch_wait_count             
page_io_latch_wait_in_ms             
tree_page_latch_wait_count           
tree_page_latch_wait_in_ms           
tree_page_io_latch_wait_count        
tree_page_io_latch_wait_in_ms        
page_compression_attempt_count       
page_compression_success_count       
version_generated_inrow              
version_generated_offrow             
ghost_version_inrow                  
ghost_version_offrow                 
insert_over_ghost_version_inrow      
insert_over_ghost_version_offrow     
";
}
