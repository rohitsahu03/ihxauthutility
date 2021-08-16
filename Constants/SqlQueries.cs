namespace AuthUtility.Constants
{
    public class SqlQueries
    {
        public const string UpdateEmailAuth = "update TblApplicationUser set TAU_EmailId='{0}',Modifiedon=GETDATE(),TAU_ModifiedBy={1} where TAU_Id={2}";
        public const string UpdateMobileNo = "update TblApplicationUser set TAU_PhoneNumber='{0}',Modifiedon=GETDATE(),TAU_ModifiedBy={1} where TAU_Id={2}";
        public const string UnlockUsers = "update TblApplicationUser set TAU_IsLocked=0,TAU_FailedAttemptCount=0,Modifiedon=GETDATE(),TAU_ModifiedBy={0},TAU_AccountLockedOn='' where TAU_Id in ({1})";
        public const string UpdateActiveStatusAuth = "update TblApplicationUser set TAU_IsActive={0},Modifiedon=GETDATE(),TAU_ModifiedBy={1} where TAU_Id in ({2})";
        public const string UpdateLoginName = "update TblApplicationUser set TAU_LoginName='{0}',Modifiedon=GETDATE(),TAU_ModifiedBy={1} where TAU_Id={2}";
        public const string UpdatePasswordinAuth = "update TblApplicationUser set TAU_Password=@Password,TAU_IsLocked=0,TAU_FailedAttemptCount=0,Modifiedon=GETDATE(),TAU_ModifiedBy=@ModifiedBy where TAU_Id in ({0})";

        public const string GetAllRolesinAuth = "select Id,Name,GroupKey,IsActive,IsAdminPrivilege from Role where IsActive=1 order by Name";
        public const string MergeQueryProperty = @"MERGE TblApplicationUserProperty as T
                                             USING @SourceTable as S
                                             on T.TAUP_TAU_Id=S.Id and T.TAUP_Name=S.Name
                                             When MATCHED THEN
                                             update set T.TAUP_Value=S.Value,T.TAUP_ModifiedOn=GETDATE(),T.TAUP_ModifitedBy = @UserTauId
                                             WHEN NOT MATCHED BY TARGET THEN
                                             INSERT(TAUP_TAU_Id,TAUP_Name,TAUP_Value,TAUP_IsActive,TAUP_CratedByApp,TAUP_CreatedOn,TAUP_CreatedBy,TAUP_ModifiedOn,TAUP_ModifitedBy)
                                             VALUES(S.Id,S.Name,S.Value,1,@AppId,GETDATE(),@UserTauId,GETDATE(),@UserTauId);";
        public const string GetDbNameByPolId = @"DECLARE @DB varchar(20)=null
                                             BEGIN
                                             IF EXISTS(SELECT 1 from MEDIASSIST..tblmapolicy where PolID={0})
                                                 SET  @DB = 'MEDIASSIST'     
                                             ELSE IF EXISTS(SELECT 1 from MEDIASSISTUHS..tblmapolicy where PolID={0})
                                                 SET  @DB = 'MEDIASSISTUHS'     
                                             END
                                             select @DB";
        public const string MergeQueryRole = @"MERGE TblUserMap_Role as T
                                          USING @SourceTable as S
                                          on T.TUMR_TAU_Id=S.Id and T.TUMR_Role=S.Role
                                          When MATCHED AND S.IsActive <> T.Tumr_IsActive THEN
                                          update set T.TUMR_IsActive=S.IsActive,T.TUMR_ModifiedOn=GETDATE(),T.TUMR_ModifiedBy= @UserTauId
                                          WHEN NOT MATCHED BY TARGET AND S.IsActive=1 THEN
                                          insert (TUMR_TAU_Id,TUMR_IsActive,TUMR_Role,TUMR_CreatedBy,TUMR_CreatedOn,TUMR_ModifiedBy,TUMR_ModifiedOn)
                                          values(S.Id,1,S.Role,@UserTauId,GETDATE(),@UserTauId,GETDATE());";

        public const string DeactivatePreviousRolesinAuth = @"UPDATE TblUserMap_Role set TUMR_IsActive=0,TUMR_ModifiedOn=GETDATE(),TUMR_ModifiedBy= @UserTauId where TUMR_TAU_Id in (select distinct Id from @SourceTable)";
        public const string GetAllBrokers = "select Id,BrokerName from MEDIASSIST..tblmabroker order by BrokerName";
        public const string sp_brokerLogin = "Create_Broker_Login_in_Auth";
        public const string GetAllProclaimRoles = "select Id,Name from pc.mstRole where IsActive=1 order by Name";

        public const string MergeQueryProclaimRole = @"MERGE pc.tblUser_Role_Map as T
                                                   USING @SourceTable as S
                                                   on T.UserId=S.Id and T.Roleid=S.Role
                                                   When MATCHED AND S.IsActive <> T.IsActive THEN
                                                   update set T.IsActive=S.IsActive,T.ModifiedOn=GETDATE(),T.ModifiedBy= @UserTauId
                                                   WHEN NOT MATCHED BY TARGET AND S.IsActive=1 THEN
                                                   insert (UserID,Roleid,IsActive,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)
                                                   values(S.Id,S.Role,1,@UserTauId,GETDATE(),@UserTauId,GETDATE());";
        public const string DeactivatePreviousRolesinProclaim = @"UPDATE pc.tblUser_Role_Map set IsActive=0,ModifiedOn=GETDATE(),ModifiedBy= @UserTauId 
                                                      where UserID in (select distinct Id from @SourceTable)";
        public const string GetPropertiesFromCMS = @"DECLARE @Format varchar(20)='{0}'
                        if OBJECT_ID('tempdb..#tblProperty') is not null drop table #tblProperty 
                        CREATE TABLE #tblProperty(TAU_Id int,empcodeORemail varchar(500))
                        IF(@Format='EmpCode') 
                        BEGIN 
                           insert into #tblProperty 
                           SELECT TAU_Id,CASE WHEN charindex('@',TAU_LoginName) > 0 
                           THEN substring(TAU_LoginName,1,charindex('@',TAU_LoginName)-1)
                           ELSE TAU_LoginName END empcodeORemail
                           FROM Mediauth..TblApplicationUser where tau_id in ({1})
                           
                           select Id,Name,Value 
                           from(
                           select TAU_Id as Id,
                           convert(varchar(25),PriBenefEmpCode) collate SQL_Latin1_General_CP1_CI_AS as IWP_EmpID, 
                           convert(varchar(25),ISNULL(BenefSex,'')) collate SQL_Latin1_General_CP1_CI_AS as Gender,
                           convert(varchar(25),ISNULL(BenefDOB,''),105) collate SQL_Latin1_General_CP1_CI_AS as DOB,
                           convert(varchar(25),BenefMediAssistID) collate SQL_Latin1_General_CP1_CI_AS as MAID 
                           from #tblProperty t 
                           join {2}..tblmapribeneficiary (nolock) p on t.empcodeORemail collate SQL_Latin1_General_CP1_CI_AS=p.PriBenefEmpCode 
                           and p.PriBenefPolID = {3}
                           join {2}..tblmabeneficiary (nolock) b on b.BenefPriID=p.PriBenefID and b.BenefRelToPriID =1 
                           and p.PriBenefEmpCode in (t.empcodeORemail collate SQL_Latin1_General_CP1_CI_AS)) as source
                           UNPIVOT
                           (Value For Name in (
                           IWP_EmpID, 
                           Gender,
                           DOB,
                           MAID)) as unpivoted
                        END
                        ELSE
                        BEGIN
                           insert into #tblProperty 
                           SELECT TAU_Id,TAU_EmailId as empcodeORemail
                           FROM Mediauth..TblApplicationUser where tau_id in ({1})
                           
                           select Id,Name,Value 
                           from(
                           select TAU_Id as Id,
                           convert(varchar(25),PriBenefEmpCode) collate SQL_Latin1_General_CP1_CI_AS as IWP_EmpID, 
                           convert(varchar(25),ISNULL(BenefSex,'')) collate SQL_Latin1_General_CP1_CI_AS as Gender,
                           convert(varchar(25),ISNULL(BenefDOB,''),105) collate SQL_Latin1_General_CP1_CI_AS as DOB,
                           convert(varchar(25),BenefMediAssistID) collate SQL_Latin1_General_CP1_CI_AS as MAID 
                           from #tblProperty t 
                           join {2}..tblmabeneficiary (nolock) b on t.empcodeORemail collate SQL_Latin1_General_CP1_CI_AS=b.BenefEmail
                           and b.BenefPolID={3} and b.BenefEmail in (t.empcodeORemail collate SQL_Latin1_General_CP1_CI_AS) 
                           join {2}..tblmapribeneficiary (nolock) p on b.BenefPriID=p.PriBenefID and b.BenefRelToPriID =1 ) as source
                           UNPIVOT
                           (Value For Name in (
                           IWP_EmpID, 
                           Gender,
                           DOB,
                           MAID)) as unpivoted
                        END";
        public const string ChangeCorporateAlias = @"update tblApplicationUser set Tau_loginname=REPLACE (Tau_loginname, '{0}', '{1}'),TAU_ModifiedBy={2},Modifiedon=GETDATE()
                                                              where TAU_ProviderMasterEntityId={3} and Tau_loginname like '%@{0}'";

        public const string InsertPasswordInSecondColumn = @"UPDATE TblApplicationUser SET TAU_Password1 = ENCRYPTBYPASSPHRASE('MediAuth', '{1}'),Modifiedon=GETDATE() WHERE TAU_Id in ({0})";

        public const string GetDuplicateUserNames = @"if OBJECT_ID('tempdb..#t') is not null drop table #t
                                                    select * into #t from mediauth..tblapplicationuser (nolock) where TAU_ProviderMasterEntityId={0} and TAU_LoginName like '%{1}'
                                                    select COUNT(*) from mediauth..tblapplicationuser (nolock) where TAU_LoginName in (select replace(TAU_LoginName,'{1}','{2}') from #t)
                                                    and TAU_LoginName not like 'hr@%' and TAU_LoginName not like 'crm@%'";

        public const string ChangeProviderMasterEntityId = @"update tblapplicationuser set TAU_ProviderMasterEntityId={2},TAU_ModifiedBy={3},Modifiedon=GETDATE() where TAU_ProviderMasterEntityId={1} and TAU_Id in ({0})";

        public const string CheckIfEntityIdIsCorrect = @"select E_FullName from IHXSupreme..ENTITY where E_Id={0}";

        public const string UpdateIsMobileVerified = "update tblapplicationuser set TAU_IsMobileVerified={0},TAU_MobileVerifiedModifiedOn='{1}',TAU_ModifiedBy={2} where TAU_Id in ({3})";

        public const string UpdateIsEmailVerified = "update tblapplicationuser set TAU_IsEmailVerified={0},TAU_EmailVerifiedModifiedOn='{1}',TAU_ModifiedBy={2} where TAU_Id in ({3})";

        public const string GetTauIdsFromEntityId = @"DECLARE	@Offset INT = {0}
                                                      DECLARE @Size INT = 500
                                                      SELECT  STUFF((SELECT Top (@Size) ','+CAST (TAU_Id as varchar(20)) 
                                                      FROM
                                                      (
                                                      	SELECT	TAU_Id,
                                                      			ROW_NUMBER() OVER (ORDER BY TAU_Id)	AS	RowNumber
                                                      	FROM	TblApplicationUser (nolock) where TAU_ProviderMasterEntityId={1} and TAU_IsActive=1
                                                      ) Selection 
                                                      WHERE	Selection.RowNumber >  (@Offset-1) * @Size
                                                      FOR XML PATH ('')),1,1,'')  as UserIds";

        public const string UpdateCorpAliasWithPolId = @"DROP TABLE IF EXISTS #t
                                                         select u.TAU_LoginName into #t from {2}..tblmapribeneficiary pb join 
                                                         (select substring(TAU_LoginName,1,charindex('@',TAU_LoginName)-1) EmpId,TAU_LoginName from 
                                                         TblApplicationUser where TAU_ProviderMasterEntityId={3} and TAU_LoginName like '%{0}') u 
                                                         on u.EmpId collate database_default=pb.PriBenefEmpCode and PriBenefPolID= {4}
                                                         
                                                         update u 
                                                         set TAU_LoginName=REPLACE (u.TAU_LoginName, '{0}', '{1}'),TAU_ModifiedBy={5},Modifiedon=GETDATE()
                                                         from tblApplicationUser u 
                                                         join #t on u.TAU_LoginName = #t.TAU_LoginName ";

        public const string GetAliasMapping = @"select CAM_ID, CAM_PolCorporateId, CAM_PolGroupCorporateId, CAM_DBType, CAM_Prefix, CAM_LoginType, CAM_Suffix, CAM_PossibleAliases, CAM_IsActive, CAM_CreatedOn, CAM_CreatedBy, CAM_ModifiedOn, CAM_ModifiedBy, PasswordType from tblCorporateAliasMapping(nolock) where CAM_PolCorporateId={0} and CAM_IsActive = 1";

        public const string SpUserDetail = "Usp_GetUserDetails";

        public const string GetTauIds = @"select TAU_Id from TblApplicationUser (nolock) where TAU_ProviderMasterEntityId={0} and TAU_IsActive=1";

        //public const string UpdateDomainNameinAliasMapping = @"update tblCorporateAliasMapping set CAM_Suffix='{0}' where CAM_PolCorporateId={1}";

        public const string GetEntityIdFromUserId = @"select TAU_ProviderMasterEntityId from tblapplicationuser (nolock) where TAU_Id={0}";

        public const string GetEntityIdFromPolId = @"select Pol_CorporateId from {0}..tblmapolicy (nolock) where PolID={1}";

        //public const string UpdateLoginNameIfChangedEmpId = @"update U set TAU_LoginName=
        //                                                    CASE WHEN substring(TAU_LoginName,1,charindex('@',TAU_LoginName)-1) <> P.TAUP_Value
        //                                                    THEN REPLACE(TAU_Loginname,substring(TAU_LoginName,1,charindex('@',TAU_LoginName)-1),P.TAUP_Value)
        //                                                    ELSE
        //                                                    TAU_LoginName END,
        //                                                    TAU_ProviderMasterEntityId=
        //                                                    CASE WHEN TAU_ProviderMasterEntityId<>@EntityId
        //                                                    THEN @EntityId 
        //                                                    ELSE
        //                                                    TAU_ProviderMasterEntityId END,
        //                                                    TAU_ModifiedBy=@UserTauId
        //                                                    from tblapplicationuser (nolock) U
        //                                                    join @SourceTable S on U.Tau_id=S.Id and S.Name='IWP_EmpID'
        //                                                    join TblApplicationUserProperty (nolock) P on U.TAU_Id=P.TAUP_TAU_Id and P.TAUP_Name='IWP_EmpID'";

        public const string SaveAlias = @"IF EXISTS (SELECT * FROM tblCorporateAliasMapping WHERE CAM_PolCorporateId={0})
                                             UPDATE tblCorporateAliasMapping SET CAM_Suffix='{1}',CAM_ModifiedBy= {2} WHERE CAM_PolCorporateId={0}
                                          ELSE
                                             INSERT INTO tblCorporateAliasMapping(CAM_PolCorporateId,CAM_PolGroupCorporateId,CAM_DBType,CAM_Prefix,CAM_LoginType,CAM_Suffix,CAM_PossibleAliases,CAM_IsActive,CAM_CreatedOn,CAM_CreatedBy,CAM_ModifiedOn,CAM_ModifiedBy,PasswordType) 
                                             VALUES ({0},null,null,null,'EMPID','{1}',null,1,GETDATE(),{2},GETDATE(),{2},null)";

        public const string UpdateEntityIdFromCMS = @"update TblApplicationUser set TAU_ProviderMasterEntityId={0},TAU_ModifiedBy={1},Modifiedon=GETDATE() where TAU_Id in ({2}) and TAU_ProviderMasterEntityId <> {0}";

        //public const string ChangePassword = @"UPDATE TblApplicationUser set TAU_Password=@byteArrPass,
        //                                       Tau_IsLocked=0,Tau_FailedAttemptCount=0,Modifiedon=GETDATE(),
        //                                       TAU_Password1 = case when @logPass =1 then ENCRYPTBYPASSPHRASE('MediAuth', @pass) else TAU_Password1 end
        //                                       where TAU_LoginName=@username";

        public const string UpdateBulkUserDetails = @"BEGIN TRANSACTION
                                                        update u 
                                                        set u.TAU_LoginName =  case 
                                                                                  when s.OldUserName is not null and s.NewUserName is not null and s.OldUserName <> '' and s.NewUserName <> '' then s.NewUserName
						                                                          else u.TAU_LoginName
					                                                           end,
	                                                        u.TAU_ProviderMasterEntityId= case 
	                                                                              when  s.EntityId is not null and s.EntityId <> 0 then s.EntityId
						                                                          else u.TAU_ProviderMasterEntityId
						                                                          end,
	                                                        u.TAU_ModifiedBy = @UserTauId,
	                                                        u.Modifiedon=GETDATE()
                                                        from @SourceTable as s
                                                        join TblApplicationUser (nolock) u on s.UserId=u.TAU_Id

                                                        if OBJECT_ID('tempdb..#p') is not null drop table #p
                                                        select UserId,EmployeeId into #p from @SourceTable where EmployeeId is not null and EmployeeId <> ''

                                                        IF EXISTS (SELECT TOP 1 * FROM #p)
                                                        BEGIN
                                                            MERGE TblApplicationUserProperty as T
                                                            USING #p as S
                                                            on T.TAUP_TAU_Id=S.UserId and T.TAUP_Name='IWP_EmpID'
                                                            When MATCHED THEN
                                                              update set T.TAUP_Value=S.EmployeeId,T.TAUP_ModifiedOn=GETDATE(),T.TAUP_ModifitedBy = @UserTauId
                                                            WHEN NOT MATCHED BY TARGET THEN
                                                              INSERT(TAUP_TAU_Id,TAUP_Name,TAUP_Value,TAUP_IsActive,TAUP_CratedByApp,TAUP_CreatedOn,TAUP_CreatedBy,TAUP_ModifiedOn,TAUP_ModifitedBy)
                                                              VALUES(S.UserId,'IWP_EmpID',S.EmployeeId,1,@AppId,GETDATE(),@UserTauId,GETDATE(),@UserTauId);
                                                        END

                                                        COMMIT TRANSACTION";
    }
}