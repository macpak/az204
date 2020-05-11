####Azure storage offers different access tiers, which allow you to store blob object data in the most cost-effective manner. The available access tiers include:

1. Hot - Optimized for storing data that is accessed frequently.
2. Cool - Optimized for storing data that is infrequently accessed and stored for at least 30 days.
3. Archive - Optimized for storing data that is rarely accessed and stored for at least 180 days with flexible latency requirements (on the order of hours).

The following considerations apply to the different access tiers:

  * Only the hot and cool access tiers can be set at the account level. The archive access tier isn't available at the account level.
  * Hot, cool, and archive tiers can be set at the blob level during upload or after upload.
  * Data in the cool access tier can tolerate slightly lower availability, but still requires high durability, retrieval latency, and throughput characteristics similar to hot data. For cool data, a slightly lower availability service-level agreement (SLA) and higher access costs compared to hot data are acceptable trade-offs for lower storage costs.
  * Archive storage stores data offline and offers the lowest storage costs but also the highest data rehydrate and access costs.
  
  ####Object storage data tiering between hot, cool, and archive is only supported in Blob storage and General Purpose v2 (GPv2) account
  
  #####Hot access tier
  The hot access tier has higher storage costs than cool and archive tiers, but the lowest access costs. Example usage scenarios for the hot access tier include:
  
  Data that's in active use or expected to be accessed (read from and written to) frequently.
  Data that's staged for processing and eventual migration to the cool access tier.
  
  #####Cool access tier
  The cool access tier has lower storage costs and higher access costs compared to hot storage. This tier is intended for data that will remain in the cool tier for at least 30 days. Example usage scenarios for the cool access tier include:
  
  Short-term backup and disaster recovery datasets.
  Older media content not viewed frequently anymore but is expected to be available immediately when accessed.
  Large data sets that need to be stored cost effectively while more data is being gathered for future processing. (For example, long-term storage of scientific data, raw telemetry data from a manufacturing facility)
  
  #####Archive access tier
  The archive access tier has the lowest storage cost. But it has higher data retrieval costs compared to the hot and cool tiers. Data in the archive tier can take several hours to retrieve. Data must remain in the archive tier for at least 180 days or be subject to an early deletion charge.
  
  #####Hot and cool are available on account level
  #####Hot/cool/archive are available on object (blob) level