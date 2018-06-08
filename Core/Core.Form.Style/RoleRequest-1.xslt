<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl frm"
    xmlns:frm="abyssaldataprocessor/forms/rolerequest/v1"
>
  <xsl:output method="html" indent="no"/>
  <xsl:template match="/">
    <xsl:apply-templates select="frm:RoleRequest"/>
  </xsl:template>
  <xsl:template match="frm:RoleRequest">
    <div>
      <label>Full Name</label>:
      <span>
        <xsl:value-of select="frm:FullName"/>
      </span>
    </div>
    <div>
      <label>Comment</label>:
      <p style="display: inline;">
        <xsl:value-of select="frm:Comment"/>
      </p>
    </div>
  </xsl:template>
</xsl:stylesheet>